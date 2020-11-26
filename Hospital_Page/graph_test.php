<?php

$mysqli_host = '192.168.0.173';
$mysqli_user = 'test';
$mysqli_password = '1234';
$mysqli_db = 'heartbeat';

// 접속
$conn = mysqli_connect($mysqli_host, $mysqli_user, $mysqli_password);
$dbconn = mysqli_select_db($mysqli_db, $conn);


// charset 설정, 설정하지 않으면 기본 mysqli 설정으로 됨, 대체적으로 euc-kr를 많이 사용
//mysqli_query("set names utf8");


$sql="
select * from (
SELECT DATE_FORMAT( regday,  '%m-%d %HH' )  mdh , COUNT( * ) cnt, SUM( temperature ) , round(SUM( temperature ) / COUNT( * ),2)  atemper
FROM  `temperature`
GROUP BY DATE_FORMAT( regday,  '%Y%m%d%H' )
order by regday desc
limit 12 
) t_a
order by t_a.mdh
";
//echo $sql;

 

$result = mysqli_query($sql) ;


$str_mdh="";
$str_atemper="";
while ($row = mysqli_fetch_array($result, mysqli_ASSOC)) {
// echo($row['mdh']."--------------".$row['atemper']."<br>");
 $str_mdh .="'".$row['mdh']."',";
 $str_atemper .="".$row['atemper'].",";
}
$str_mdh= substr($str_mdh,0,-1);
$str_atemper= substr($str_atemper,0,-1);
//echo $str_atemper;

?><!DOCTYPE HTML>
<html>
 <head>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
  <title>Temperature Example</title>

  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
  <style type="text/css">
${demo.css}
  </style>
  <script type="text/javascript">
$(function () {
    $('#container').highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: 'Average Temperature'
        },
        subtitle: {
            text: 'Source: ilikesan.com'
        },
        xAxis: {
            categories: [<?php echo $str_mdh?>]
        },
        yAxis: {
            title: {
                text: 'Temperature (°C)'
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        series: [{
            name: 'Home',
            data: [<?php echo $str_atemper?>]
        }
   /*
   , {
            name: 'London',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
        }
  */
  ]
    });
});
  </script>
 </head>
 <body>
<script src="/highchart/js/highcharts.js"></script>
<script src="/highchart/js/modules/exporting.js"></script>

<div id="container" style="min-width: 310px; height: 400px; margin: 0 auto"></div>

 </body>
</html>
