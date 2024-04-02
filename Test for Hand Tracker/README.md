Handpose는 그리지 않도록 변경해둔 상태
Handpose를 확인하고 싶다면 아래 부분을 주석 해체하고
![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/93fbeae6-2af3-4cdb-bb7b-7369f4e1c5cf)

아래와 같이 Mesh, Material이 잘 입력돼있는지 확인 후 실행
![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/46ceaa10-b3b3-4a92-8f2c-e70048d90a0f)


===============================================================


![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/b8bceecf-38bf-4f46-bf6f-19a01b85959b)


-Canvas : "Monitor" 이름의 RawImage를 웹캠 입력으로 받기 위해 사용

-Camera ~ Wall : HandPose의 시각화 및 시각성을 높이기 위해 사용 

-Animator : Barracuda 모델링을 실행시키는 Object로 웹캠과 HandPose를 관리

-running : 정상적으로 Hand KeyPoint를 받아오는지 확인하기 위해 넣은 이미지 오브젝트

================================================================

![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/5682d6d7-4ba0-44b5-99c0-c06f4fc7908c)

KeyPoint를 Vector3 형식으로 받아오며, 

GameObject.Find("오브젝트").GetComponent<스크립트>().GetPoint(인덱스);

형태로 사용한다.

![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/f63f025b-fa04-4370-9606-872f4a039514)

위 사진을 참고하여 필요한 좌표의 인덱스를 입력해주면 된다.

=================================================================

Scene을 불러오지 않고 

직접 Object를 추가하여 스크립트를 넣는 경우, UI - RawImage로 RawImage를 하나 만든 다음 아래와 같이 연결해주면 된다.

![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/acd290a4-292e-4fd5-ad1d-059a182af206)


GameObject.Find("오브젝트").GetComponent<스크립트>().GetPoint(인덱스);

형식으로 KeyPoint를 읽어오니 Find 함수의 오브젝트명과 추가한 오브젝트명을 통일해줘야 한다.


![image](https://github.com/BaekJunehong/Hufs_Capstone/assets/163231781/1ddd3df1-ba95-4fd2-956e-03d4d1967bdd)

콘솔 로그에 위와 같이 표시된다면 정상적으로 작동중인 것이다.


