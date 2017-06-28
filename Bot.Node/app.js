require('dotenv-extended').load();

var restify = require('restify');
var builder = require('botbuilder');
var request = require('request');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
   console.log('%s listening to %s', server.name, server.url); 
});

// Create chat connector for communicating with the Bot Framework Service
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

// Listen for messages from users 
server.post('/api/messages', connector.listen());

var bot = new builder.UniversalBot(connector);

bot.dialog('/', function (session) {
	switch (session.message.text.toLowerCase()) {
		case 'help':
			session.replaceDialog('Welcome');
			break;
	
		default:
			//session.send('Hi, welcome to the Node Bot!')
			session.replaceDialog('LUIS');
			break;
	}
});

bot.on('conversationUpdate', function (session) {
    if (session.membersAdded) {
        session.membersAdded.forEach((identity) => {
            if (identity.id === session.address.bot.id) {
				bot.beginDialog(session.address, 'Welcome');
            }
        });
    }
});

// var qnarecognizer = new cognitiveservices.QnAMakerRecognizer({
// 	knowledgeBaseId: 'set your kbid here', 
// 	subscriptionKey: 'set your subscription key here',
//     top: 4});

var model = process.env.LUIS_MODEL_URL;
var recognizer = new builder.LuisRecognizer(model);

//=========================================================
// Bot Dialogs
//=========================================================
var intents = new builder.IntentDialog({ recognizers: [recognizer] }); //, qnarecognizer] });
bot.dialog('LUIS', intents);
var url = 'https://jsonplaceholder.typicode.com/albums';
intents.matches('None', function(session, args){
			request(url, (error, response, body)=> {
			if (!error && response.statusCode === 200) {
				const jsonResponse = JSON.parse(body)
				jsonResponse.forEach(function(albums) {
					 session.send(albums.title);
				}, this);

				//console.log("Got a response: ", fbResponse.title)
			} else {
				console.log("Got an error: ", error, ", status code: ", response.statusCode)
			}
			});

	        session.send('This is the none intent');
			session.endDialog();
});

// intents.matches('qna', [
//     function (session, args, next) {
//         var answerEntity = builder.EntityRecognizer.findEntity(args.entities, 'answer');
//         session.send(answerEntity.entity);
//     }
// ]);

intents.onDefault([
    function(session){
        session.send('Sorry!! No match!!');
	}
]);

bot.dialog('Welcome', function (session) {
	var card = new createHeroCard(session);
	var msg = new builder.Message(session).addAttachment(card);
    session.send(msg);
	session.beginDialog('LUIS', session);	
});

function createHeroCard(session) {
    return new builder.HeroCard(session)
        .title('BotFramework Hero Card')
        .subtitle('Your bots — wherever your users are talking')
        .text('Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.')
        .images([
            builder.CardImage.create(session, 'https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg')
        ])
        .buttons([
            builder.CardAction.openUrl(session, 'https://docs.microsoft.com/bot-framework/', 'Get Started')
        ]);
}