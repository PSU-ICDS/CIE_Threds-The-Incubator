<?php
   $text1 = $_POST["info_Total"];
   $text2 = $_POST["info_Responses"];
   $text3 = $_POST["info_GameStats"];

if($text1 != "")
{
echo("Message successfully sent!");
echo("Field 1:" . $text1);
echo("Field 2: " . $text2);
echo("Field 3: " . $text3);
$file = fopen("data.txt", "a");
fwrite($file, $text1);
fwrite($file, $text2);
fwrite($file, $text3);
fclose($file);
} else
{
echo("Message delivery failed...");
}
?>