<?php

$connect = @mysqli_connect("192.168.0.173","test","1234","heartbeat");

$seq = $_POST['seq'];

mysqli_set_charset($connect,"utf8");


	echo "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><script language=javascript>";
	echo "if(confirm('해당 기록을 삭제하시겠습니까?'))";
	echo "{";
	
	$query = "delete from hospital_emer_check where seq =" .$seq."";
	$result = mysqli_query($connect,$query) or die(mysqli_error($connect));


	$connect->close();
	echo 'location.href="main.php"';
	
	echo "}";
	echo "else";
	echo "{";
	echo "history.back(-2);";
	echo "}";
	echo "</script>";


?>