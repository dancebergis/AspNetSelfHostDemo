# Requirements done [+], not done [-]:

[-] It has its own database where municipality taxes are stored
[+] Taxes should have ability to be scheduled (yearly, monthly ,weekly ,daily) for each municipality
[-] Application should have ability to import municipalities data from file (choose one data format you believe is suitable)
[+] Application should have ability to insert new records for municipality taxes (one record at a time)
[+] User can ask for a specific municipality tax by entering municipality name and date
[+] Errors needs to be handled i.e. internal errors should not to be exposed to the end user
[+] You should ensure that application works correctly

# Extra:

* Application is deployed as a self-hosted windows service > if you build TaxManager project and run it as a service ?
* Update record functionality is exposed via API > if webService that accepts HTTP POST is ok?

# Usage:

"Producer service" : Build TaxManager. Run.
"Consumer service" : here it is -> https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop. Use it to do HTTP requests. Samples are included (Tax test.postman_collection.json)

Examples:

1. GET http://localhost:8080/api/tax/c/2016.11.01 -> gets taxes for city "c" on date "2016.11.01"
2. POST http://localhost:8080/api/tax with JSON body:
{	
	"City": "b",
	"Tax": 999.6,
	"Day": "2016-10-10"
}
-> inserts new tax (999.6) for city "b" on date "2016.10.10"

3. POST http://localhost:8080/api/tax with JSON body:
 {	
	"City": "b",
	"Tax": -22,
	"Year": 2016,
	"Month": 10
}
-> inserts new tax -22 for city "b" on "2016.10" month


4. POST http://localhost:8080/api/tax with JSON body:
{	
	"City": "b",
	"Tax": 5,
	"Year": 2016
}
-> inserts new tax 5 for city "b" on 2016 year

5. POST http://localhost:8080/api/tax with JSON body:
{	
	"City": "b",
	"Tax": 33,
	"Year": 2016,
	"WeekOfYear": 2
}
-> inserts new tax 33 for city "b" on 2nd week of 2016 year.