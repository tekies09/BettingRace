# happyhouse-hmt [혜택조]
## 목차
  - [개요](#개요) 
  - [일정및팀원](#일정및팀원) 
  - [설치가이드](#설치가이드)
  - [기능리스트](#기능리스트)
  - [주요화면](#주요화면)
  - [기술스택및환경](#기술스택및환경)
  - [레퍼런스](#레퍼런스)

## 개요

**프로젝트 목적 및 용도는 다음과 같다.**
- 공공API를 이용한 데이터 가공
- 가공된 데이터를 구글 및 카카오 맵 API를 연동하여 데이터 시각화
- 주변 상권 및 진료소 등의 해당 아파트 가격에 영향을 줄수있는 데이터 시각화
- 회원제 운영 및 회원 정보 통계하여 차트를 시각화
- 게시판 운영을 통한 회원간의 정보 교류 제공

**산출물 (화면 설계서 , 클래스다이어그램 등...)**
- [자세히](https://github.com/tekies09/Happyhouse/tree/main/outputs)


## 일정및팀원
**📆 프로젝트 기간**
- 2021년 07월 18일 - 2021년 11월 17일 (4개월)

**프로젝트 역할 및 직책**

|직책|이름|역할|
|------|---|---|
|팀장|노문택|게시판 , 회원관리 , 데이터 시각화 (차트) , 아파트 API 데이터 가공 |
|조원|이혜민|카카오 및 구글 API 연동 , 주변 상권 API 데이터 가공 및 활용 |

## 설치가이드
**프론트엔드의 경우만 다음과 같이 Bash 창에서 입력해주면 된다.**
- [자세히](https://github.com/tekies09/Happyhouse/tree/main/frontend)

## 기능리스트
**메인기능**
- 회원가입 
- 로그인 및 정보 조회
- 관리자 모드 [ 회원 관리 , 회원 통계 , 공지사항  및 자유게시판 관리 ]
- 아파트 정보 조회
- 주변 상권 조회

## 주요화면
**주요 화면 4컷 미리보기**  
  
![캡처](https://github.com/tekies09/Happyhouse/blob/main/img/result.JPG)

- [자세히](https://github.com/tekies09/Happyhouse/tree/main/pages)

## 기술스택및환경

**기술스택**

- Backend  
<img src="https://img.shields.io/badge/JAVA-007396?style=for-the-badge&logo=java&logoColor=white">  <img src="https://img.shields.io/badge/Spring-6DB33F?style=for-the-badge&logo=Spring&logoColor=white"> <img src="https://img.shields.io/badge/mysql-4479A1?style=for-the-badge&logo=mysql&logoColor=white">  

- Frontend  
<img src="https://img.shields.io/badge/bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white"> <img src="https://img.shields.io/badge/vue.js-4FC08D?style=for-the-badge&logo=vue.js&logoColor=white"> <img src="https://img.shields.io/badge/javascript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black">  

**개발환경**

|환경|버전|설명|
|------|---|---|
| JAVA | zulu 1.8 | Backend |
| mybatis | 2.1.3 | Backend |
| Springboot | 2.6.12 | Backend |
| MySQL | 8.0.21 | DB |
| Vue.js | 2.6.12 | Frontend |

## 레퍼런스
**레퍼런스**
- kakao Map API: https://apis.map.kakao.com/web/sample/categoryFromBounds/ https://velog.io/@geommyeong/Vue.js-KakaoMap-API


