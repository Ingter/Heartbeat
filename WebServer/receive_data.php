<html lang="ko">
<head>
	<meta charse="utf-8">
</head>
<body>
<?php

?>
</body>
</html>

<?php header("content-type:text/html; charset=UTF-8");
error_reporting(E_ALL);
ini_set("display_errors", 1);

$connect = @mysqli_connect("192.168.0.173","test","1234","heartbeat");

if(!$connect)
	die("connect fail.".mysqli_error());

else{
	echo "connect success.<br>\n";

date_default_timezone_set('Asia/Seoul');
$emp_id = $_GET['emp_id'];
$heart_rate = $_GET['heart_rate'];
$tem_rate = $_GET['tem_rate'];
$time = date("Y-m-d H:i:s");

echo $time. "<br>";

mysqli_set_charset($connect,"utf8");

if($heart_rate == 0 || $heart_rate == 1 || $tem_rate < 30 || $tem_rate > 45)
{

}


else if($heart_rate <130 && $heart_rate > 40 && $tem_rate > 34 && $tem_rate < 38)
{
	$query = "insert into real_time_data(emp_id,heart_rate,tem_rate,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

	$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

}


else
{

	$query = "insert into real_time_data(emp_id,heart_rate,tem_rate,time) values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

	$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

	$query = "insert into emp_emer(emp_id,heart_rate,body_temp,time) values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

	$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

	$query = "insert into hospital_emer_check(emp_id,heart_rate,body_temp,time) values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

	$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

}





$connect->close();

}
?>
