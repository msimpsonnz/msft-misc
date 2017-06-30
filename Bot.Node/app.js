require('dotenv-extended').load();

var restify = require('restify');
var builder = require('botbuilder');
var cognitiveservices = require('botbuilder-cognitiveservices');
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
bot.dialog('/');

// Trigger secondary dialogs when 'settings' or 'support' is called
bot.use({
    botbuilder: function (session, next) {
        var text = session.message.text;

        var settingsRegex = localizedRegex(session, ['luis']);
        var supportRegex = localizedRegex(session, ['help']);

        if (settingsRegex.test(text)) {
            // interrupt and trigger 'settings' dialog 
            return session.beginDialog('LUIS');
        } else if (supportRegex.test(text)) {
            // interrupt and trigger 'help' dialog
            return session.beginDialog('Welcome');
        }

        // continue normal flow
        next();
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

var qnarecognizer = new cognitiveservices.QnAMakerRecognizer({
	knowledgeBaseId: process.env.MICROSOFT_QNA_KB,
	subscriptionKey: process.env.MICROSOFT_QNA_KEY,
    top: 4});

var model = process.env.LUIS_MODEL_URL;
var recognizer = new builder.LuisRecognizer(model);

//=========================================================
// Bot Dialogs
//=========================================================
var intents = new builder.IntentDialog({ recognizers: [recognizer, qnarecognizer] });
bot.dialog('LUIS', intents);
var url = 'https://jsonplaceholder.typicode.com/albums';
intents.matches('Get_Data', function(session, args){
	//var msg = '';
			request(url, (error, response, body)=> {
			if (!error && response.statusCode === 200) {
				const jsonResponse = JSON.parse(body)
				for (index = 0; index < 10; ++index) {
					//msg.push(jsonResponse[index].title);
					session.send(jsonResponse[index].title);
			};
			} else {
				console.log("Got an error: ", error, ", status code: ", response.statusCode)
			}
			});

	        session.send('This is the \'Get_Data\' intent');
			//session.endDialog();
	});

intents.matches('qna', [
    function (session, args, next) {
        var answerEntity = builder.EntityRecognizer.findEntity(args.entities, 'answer');
        session.send(answerEntity.entity);
    }
]);

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

// Cache of localized regex to match selection from main options
var LocalizedRegexCache = {};
function localizedRegex(session, localeKeys) {
    var locale = session.preferredLocale();
    var cacheKey = locale + ":" + localeKeys.join('|');
    if (LocalizedRegexCache.hasOwnProperty(cacheKey)) {
        return LocalizedRegexCache[cacheKey];
    }

    var localizedStrings = localeKeys.map(function (key) { return session.localizer.gettext(locale, key); });
    var regex = new RegExp('^(' + localizedStrings.join('|') + ')', 'i');
    LocalizedRegexCache[cacheKey] = regex;
    return regex;
}