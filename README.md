# Date Detector
Scans images for dates and compares them with the current date. If all of the dates in the image are earlier than the current date, the program will print invalid. If any of the dates are older than the current date, the program will print valid. 

This program utilizes Microsoft Azure cognitive serivces. In order to use this program, an Azure account as well as a subscription key for cognitive services is needed.

Configuration: 

    const string subscriptionKey = "<Subscription Key"; // subscription key for cognitive services
    const string uriBase = "https://eastus.api.cognitive.microsoft.com/vision/v2.0/ocr"; // region url 

Dates in the following formats can be read:

    
    mm/dd/yyyy
    mm-dd-yyyy
    mm/dd/yy
    mm-dd-yy

Example Use-Case:

This program can be used to detect the validity of a drivers license. Simply input the file path to the program and it will produce a response (valid/invalid). 
