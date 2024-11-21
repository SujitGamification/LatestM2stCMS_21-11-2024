<?php

$name = $_GET["name"];
$email = $_GET["email"];
$tel = $_GET["tel"];
$birthdate = $_GET["birthdate"];
$website = $_GET["website"];
$username = $_GET["username"];

/*
 * 
 * Do something with these variables
 * 
 */ 
 
 $data = array('ok'=>'1');   // I noticed that when usisng AJAX with form validation, you have
 echo json_encode($data);	// to return a json array, otherwise you get a json is null error
 
?>