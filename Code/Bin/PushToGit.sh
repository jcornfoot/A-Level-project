#!/bin/bash

#Copy contents of Scripts/ in project to Code/ in git


if cp --verbose -u ~/My\ project/Assets/Scripts/*.cs ~/school/projects/A-Level-project/Code ;
then
	echo "Files updated"
	exit
else 
	echo "Error" >&2
	exit
fi
