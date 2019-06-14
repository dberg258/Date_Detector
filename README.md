# Date_Detector
Scans images for dates and compares them with the current date. If all of the dates in the image are before the current date, the program will print invalid. If any of the dates are past the current date, the program will print valid. 

This program utilizes Microsoft Azure cognitive serivces. In order to use this program, a subscription key for cognitive services is needed.

This program can be used to detect the validity of a drivers license. Simply input the file path to the program and it will produce a response (valid/invalid). 

Dates in the following formats can be read:
  mm/dd/yyyy
  mm-dd-yyyy
  mm/dd/yy
  mm-dd-yy
