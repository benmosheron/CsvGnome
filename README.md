# CsvGnome

## A Simple but Clever CSV File Generator for Windows 10.

Specify fields and values with ":".

* ```fieldName:ValueName``` creates a field (column) with name "fieldName" and all values "ValueName"

Change the number of rows to generate by entering a number E.g. ```12``` to write twelve rows of data, this doesn't include the columns row.

* ```help``` displays help.
* ```exit``` quits.
* ```write``` writes the CSV file. The file will be written to CsvGnome's Output directory.
* ```run``` writes the CSV and quits.
* ```delete fieldName``` deletes the last entered field with name "fieldName".

Special components can be added to values:
* ```[++]``` counts up from zero.
* ```[3++]``` counts up from three.
* ```[19++2]``` counts up from nineteen in steps of two.
* ```[date]``` the date and time, at the time the file is generated.
* ```[1=>5]``` cycles the values from one to five.
* ```[0=>50, 10]``` cycles the values from zero to fifty in steps of ten.
* ```[1=>5 #position]``` cycles the values from one to five. Tagged with the ID "position", tag another cycling component with the same ID to have them cycle through all possible combinations.


* ```help special``` displays a list of available components.


### Example Input
```
12
Location:London
UniqueId:[date]_ID[++]
x:[1=>2, 1 #pos]
y:[1=>5, 2 #pos]
z:[0=>1 #pos]
```

#### Example Output

|Location|UniqueId|x|y|z|
|--------|--------|---|---|---|
|London|2016-05-15T08:54:46_ID00|1|1|0|
|London|2016-05-15T08:54:46_ID01|1|1|1|
|London|2016-05-15T08:54:46_ID02|1|3|0|
|London|2016-05-15T08:54:46_ID03|1|3|1|
|London|2016-05-15T08:54:46_ID04|1|5|0|
|London|2016-05-15T08:54:46_ID05|1|5|1|
|London|2016-05-15T08:54:46_ID06|2|1|0|
|London|2016-05-15T08:54:46_ID07|2|1|1|
|London|2016-05-15T08:54:46_ID08|2|3|0|
|London|2016-05-15T08:54:46_ID09|2|3|1|
|London|2016-05-15T08:54:46_ID10|2|5|0|
|London|2016-05-15T08:54:46_ID11|2|5|1|


### GnomeFiles

Save and load configurations of fields to GnomeFiles with ```save``` and ```load``` commands. Gnomefiles can be easily edited in a text editor. The GnomeFile "default.gnome" is automatically loaded when CsvGnome runs, you can edit this file to contain a common set of fields.

* ```gnomefiles``` displays the available gnomefiles, along with their locations.
* ```save example``` saves the current fields to "example.gnome".
* ```load example``` loads the previously saved fields from "example.gnome".