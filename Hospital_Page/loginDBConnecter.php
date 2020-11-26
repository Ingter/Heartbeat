<?php
session_start();
error_reporting(E_ALL);
ini_set("display_errors", 1);
echo "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";

$id=$_POST['id'];
$pw=$_POST['pw'];
$mysqli=mysqli_connect("192.168.0.173","test","1234","heartbeat");
 
$check="SELECT * FROM hospital_manager WHERE id='$id'";
$result=$mysqli->query($check); 

if($result->num_rows==1){
    $row=$result->fetch_array(MYSQLI_ASSOC); //하나의 열을 배열로 가져오기
    if($row['pw']==$pw){  //MYSQLI_ASSOC 필드명으로 첨자 가능
        $_SESSION['id']=$id;           //로그인 성공 시 세션 변수 만들기
        if(isset($_SESSION['id']))    //세션 변수가 참일 때
        {
            header('Location: main.php?page=1');   //로그인 성공 시 페이지 이동
        }
        else{
            echo "세션 저장 실패";
        }            
    }
    else{

echo "<script language=javascript> alert('아이디나 비밀번호를 다시 확인해주세요.'); history.back(-2);</script>";
	
    }
}
else{
echo "<script language=javascript> alert('아이디나 비밀번호를 다시 확인해주세요.'); history.back(-2);</script>";

}
?>
