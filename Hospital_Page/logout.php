<?php
session_start();
session_destroy(); //모든 세션 변수 지우기

    header('Location: ./Hos_MainPage.php'); // 로그아웃 성공 시 로그인 페이지로 이동

?>
