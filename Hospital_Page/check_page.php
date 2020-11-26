<!DOCTYPE html>
<html lang="en">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Hospital Page</title>
<head>
<meta http-equiv="refresh" content="5">

<meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>



<style>

	@import url('https://fonts.googleapis.com/css2?family=Gothic+A1:wght@700&family=Noto+Sans+KR:wght@500&display=swap');
	@import url('https://fonts.googleapis.com/css2?family=Gothic+A1:wght@700&family=Noto+Sans+KR&display=swap');
	@import url('https://fonts.googleapis.com/css2?family=Do+Hyeon&display=swap');
	
	body {
	  background: #89DA63; /* fallback for old browsers */
	  background: -webkit-linear-gradient(right, #e3e3e3, #e3e3e3);
	  background: -moz-linear-gradient(right, #e3e3e3, #e3e3e3);
	  background: -o-linear-gradient(right, #e3e3e3, #e3e3e3);
	  background: linear-gradient(to left, #e3e3e3, #e3e3e3);
	  font-family: "Roboto", sans-serif;
	  -webkit-font-smoothing: antialiased;
	  -moz-osx-font-smoothing: grayscale;  
	  }
	  
	.container {
	  padding: 1em;
	}



	
	button,
	button::after {
	-webkit-transition: all 0.3s;
	-moz-transition: all 0.3s;
	-o-transition: all 0.3s;
	transition: all 0.3s;
	}
	
	button {
	background: none;
	border: 3px solid #132b60;
	border-radius: 5px;
	color: #132b60;
	display: block;
	font-size: 1.2em;
	font-weight: bold;
	margin: 0.4em auto;
	padding: 0.25em 0.75em;
	position: relative;
	text-transform: uppercase;
}

.btn-3{

	margin-right : 12px;
}

	
	.btn-3:hover:after {
	height: 100%;
	left: 0;
	top: 0;
	width: 100%;
	}
	
	button::before,
	button::after {
	background: #132b60;
	content: '';
	position: absolute;
	z-index: -1;
	}

	button:hover {
	color: #9E9C9C;
	}

	
.btn-4 {
	margin-right : 80%;
}

	.btn-4:hover:after {
	height: 100%;
	left: 0;
	top: 0;
	width: 100%;
	}

	.btn-4::before,
	.btn-4::after {
	background: #132b60;
	content: '';
	position: absolute;
	z-index: -1;
	}


input[type=submit] {
    padding:5px 15px; 
    background:#132b60; 
    border:0 none;
    cursor:pointer;
    -webkit-border-radius: 5px;
    border-radius: 5px; 
        color: #fff;
}

input[type=submit]:hover {
	    background:#ccc; 
}


h1
{
   background:#132b60;
   color: #fff;
	height: 72px;
padding-top:15px;

}

thead
{
font-size: 20px; 
font-family: 'Noto Sans KR', sans-serif;
 border-bottom:3px solid #132b60; 

}

tbody
{
font-size: 16px; 
font-family:'Noto Sans KR', sans-serif;
}

th
{

text-align:center;
}

td
{
 border-bottom:1px solid #132b60; 
text-align:center;
}

	  
</style>
</head>
<body>


<div class="Main-page">
	  <div class="container">

		 <h1 style="text-align: center; font-family:'Do Hyeon', sans-serif; font-size: 44px;">이상현상 발생 이력 조회</h1>


				 		<form class="logout" method="POST" action="logout.php">
		  <button class="btn-3" style="float: right; ">logout</button>
		</form>
		
						 		<form class="check" method="POST" action="main.php?page=1">
		  <button class="btn-4" style="float: right; ">main</button>
		  
		</form>
		<br>
		<br>
		<br>

	  <!-- 표 -->
	  <div class="container">
	  <table class="table table-condensed">
		<THEAD style="font-size: 20px; font-family: 'Noto Sans KR', sans-serif;">
			<TR>
				<th>이름</th>
				<th>연락처</th>
				<th>비상연락처</th>
				<th>혈액형</th>
				<th>체온</th>
				<th>심박수</th>
				<th>시간</th>
			</TR>                          
		</THEAD>             
		<TBODY style= "font-size: 16px; font-family:'Noto Sans KR', sans-serif;">

<?php 
session_start();
	$connect = @mysqli_connect("192.168.0.173","test","1234","heartbeat");

$mysql_query = "set names utf8";
	$data = "select emp_id from emp_emer";
$result = mysqli_query($connect,$data) or die(mysqli_error($connect));
	$data_count = mysqli_num_rows($result);

$result = mysqli_query($connect,$mysql_query) or die(mysqli_error($connect));



$i=0;

$list = 10; # 페이지 당 데이터 수
$block = 10; #블록 당 페이지 수


$page = ($_GET['page'])?$_GET['page']:1;

$pageNum = ceil($data_count/$list); # 총 페이지
$blockNum = ceil($pageNum/$block); # 총 블록
$nowBlock = ceil($page/$block);

$s_page = ($nowBlock * $block)-($block -1);


if($s_page <= 1){
	$s_page = 1;
}


	$e_page = $nowBlock*$block;
if ($pageNum <= $e_page) {
	$e_page = $pageNum;
}


$s_point = ($page-1) * $list;
	$query = "select emp_name, emp_tel, emp_emer_tel, blood_type,body_temp, heart_rate, time, seq from emp_info
right join emp_emer on emp_info.emp_id=emp_emer.emp_id order by seq desc limit ".$s_point.",".$list;
$result = mysqli_query($connect,$query) or die(mysqli_error($connect));
	$cnt = mysqli_num_rows($result);






	while($i<$cnt)
	{
		$ary = mysqli_fetch_array($result);
		echo "<tr>";
		$j = 0;
		while($j<7)
		{
		echo "<td>",$ary[$j],"</td>";
		$j = $j+1;
		}

		echo "</tr>";
		$i = $i+1;
		if($ary == false){
		break;
		}


	}
		echo '<tr>';
		echo '<td colspan = "8" align="center"><a href="check_page.php?page=',$s_page-1,'">이전</a>';
	for($p=$s_page; $p<=$e_page; $p++) {

		echo '<a href="check_page.php?page=',$p,'">',$p,'</a>';
		echo "&nbsp;";
		}
		
	
		echo '<a href="check_page.php?page=',$e_page+1,'">다음</a>';
		echo "</td>";
				echo "</tr>";

		
		
		$connect->close();
?>


		</TBODY>
	  </table>
</div>

	  <!-- 콤보박스 -->	  

	  <div>
	  <!-- 로그아웃 -->

	  </div>
	  </div>
	  
	  
	  
</div>


</body>
</html>

<?php

$connect = @mysqli_connect("192.168.0.173","test","1234","heartbeat");
$mysql_query = "set names utf8";
$result = mysqli_query($connect,$mysql_query) or die(mysqli_error($connect));

	$query = "Select count(*) from emp_emer";
	$rcds = mysqli_query($connect,$query) or die(mysqli_error($connect));
	$rcds_rows = mysqli_fetch_array($rcds);
	
		
	$query = "SELECT emp_id FROM emp_emer ORDER BY seq DESC LIMIT 1; ";
	$Emp_id = mysqli_query($connect,$query) or die(mysqli_error($connect));
	$Emp_id_rows = mysqli_fetch_array($Emp_id);


if(empty($_SESSION['count']))
{
	$_SESSION['emp_id'] = $Emp_id_rows[0];
	$_SESSION['count'] = $rcds_rows[0];



}

else
{
	$temp = $rcds_rows[0];	

	
		if ($_SESSION['count'] != $temp)  // emp_emer 테이블에 새 행이 들어왔을 때 실행
	{
		$temp = $_SESSION['count'];
		$query = "SELECT emp_id FROM emp_emer ORDER BY seq DESC LIMIT 1; ";
		$Emp_id = mysqli_query($connect,$query) or die(mysqli_error($connect));
		$Emp_id_rows = mysqli_fetch_array($Emp_id);
		


		if ($Emp_id_rows[0] != $_SESSION['emp_id'])
		{
		$_SESSION['emp_id'] = $Emp_id_rows[0];
		
		$query = "SELECT emp_name from emp_info where emp_id =".$Emp_id_rows[0];
		$Emp_name = mysqli_query($connect,$query) or die(mysqli_error($connect));
		$Emp_name_rows = mysqli_fetch_array($Emp_name);

			echo "<script>alert('{$Emp_name_rows[0]}님 이상 발생!! 관리자에게 연락해 주시기 바랍니다. ');</script>";
			echo $_SESSION['emp_id'];
			$_SESSION['count']=[];


		}

	}
}
	
	mysqli_Close($connect);

if(empty($_SESSION['id'])){

	echo "<script>alert('로그인이 필요한 서비스 입니다.');</script>";
	echo "<meta http-equiv='refresh' content='0; url=index.php'>";
	exit;

}

?>
