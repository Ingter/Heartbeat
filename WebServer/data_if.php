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

$givetime = $_GET['givetime'];
if(givetime==1){
	time.php;
}
$emp_id = $_GET['emp_id'];
$heart_rate = $_GET['heart_rate'];
$tem_rate = $_GET['tem_rate'];
$time = date("Y-m-d H:i:s");

echo $time. "<br>";

mysqli_set_charset($connect,"utf8");

//if($heart_rate == 0 || $heart_rate == 1)
//{

//}

if($heart_rate > 80 || $heart_rate < 40)
{
$query = "insert into emp_detail(emp_id,heart_rate,body_temp,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

$query = "insert into real_time(emp_id,heart_rate,tem_rate,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

}

else if( $tem_rate < 34 || $tem_rate > 37.5)
{
$query = "insert into emp_detail(emp_id,heart_rate,body_temp,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

$query = "insert into real_time(emp_id,heart_rate,tem_rate,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

}

//else if ($tem_rate <26 || $tem_rate > 50)	//온도 센서 값이 50도 이상, 26도 이하면 값 전달 안함(사람한테서 나올 수 있는 체온 범위 밖 값 전달 안함)
//{

//}

else
{
$query = "insert into real_time(emp_id,heart_rate,tem_rate,time)
	values(".$emp_id.",".$heart_rate.",".$tem_rate.","."'$time'".")";

$result = mysqli_query($connect,$query) or die(mysqli_error($connect));

}

$connect->close();

}
?>