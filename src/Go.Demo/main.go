package main

import (
    "crypto/tls"
    "encoding/json"
    "fmt"
    "log"
    "net"
    "net/http"
    "os"
    "time"
    "github.com/gorilla/mux"
    "github.com/globalsign/mgo"
    "github.com/globalsign/mgo/bson"
)

// Person Type
type Person struct {
	ID        string   `json:"id,omitempty"`
	Firstname string   `json:"firstname,omitempty"`
	Lastname  string   `json:"lastname,omitempty"`
	Address   *Address `json:"address,omitempty"`
}
// Address Type
type Address struct {
	City  string `json:"city,omitempty"`
	State string `json:"state,omitempty"`
}

var people []Person

func MongoSetup() *mgo.DialInfo {
    dialInfo := &mgo.DialInfo{
        Addrs:    []string{""}, // Get HOST + PORT
        Timeout:  60 * time.Second,
        Database: "mongo",                                                                             // It can be anything
        Username: "",                                                                             // Username
        Password: "", // PASSWORD
        DialServer: func(addr *mgo.ServerAddr) (net.Conn, error) {
            return tls.Dial("tcp", addr.String(), &tls.Config{})
        },
    }
    return dialInfo
}

// GetPeople - Display all from the people var
func GetPeople(w http.ResponseWriter, r *http.Request) {
    //json.NewEncoder(w).Encode(people)
    mgoConfig := MongoSetup()
    session, err := mgo.DialWithInfo(mgoConfig)
    if err != nil {
        fmt.Printf("Can't connect to mongo, go error %v\n", err)
        os.Exit(1)
    }  
    result := Person{}
    c := session.DB("mongo").C("coll")
    //results := c.Find(bson.M{"id": "0b5d4bb2-ca28-d787-a750-fdbec51a265c"}).One()
    err = c.Find(bson.M{"id": "0b5d4bb2-ca28-d787-a750-fdbec51a265c"}).One(&result)
	if err != nil {
		log.Println(err.Error())
    }
    //fmt.Printf(result)
    json.NewEncoder(w).Encode(result)
}

// GetPerson - Display a single data
func GetPerson(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	for _, item := range people {
		if item.ID == params["id"] {
			json.NewEncoder(w).Encode(item)
			return
		}
	}
	json.NewEncoder(w).Encode(&Person{})
}

// CreatePerson - create a new item
func CreatePerson(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	var person Person
	_ = json.NewDecoder(r.Body).Decode(&person)
	person.ID = params["id"]
	people = append(people, person)
	json.NewEncoder(w).Encode(people)
}

// DeletePerson - Delete an item
func DeletePerson(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	for index, item := range people {
		if item.ID == params["id"] {
			people = append(people[:index], people[index+1:]...)
			break
		}
		json.NewEncoder(w).Encode(people)
	}
}

// main function to boot up everything
func main() {
	router := mux.NewRouter()
	router.Use(commonMiddleware)
	people = append(people, Person{ID: "1", Firstname: "John", Lastname: "Doe", Address: &Address{City: "City X", State: "State X"}})
	people = append(people, Person{ID: "2", Firstname: "Koko", Lastname: "Doe", Address: &Address{City: "City Z", State: "State Y"}})
	router.HandleFunc("/people", GetPeople).Methods("GET")
	router.HandleFunc("/people/{id}", GetPerson).Methods("GET")
	router.HandleFunc("/people/{id}", CreatePerson).Methods("POST")
	router.HandleFunc("/people/{id}", DeletePerson).Methods("DELETE")
	log.Fatal(http.ListenAndServe(":8000", router))
}

func commonMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Add("Content-Type", "application/json")
		next.ServeHTTP(w, r)
	})
}
