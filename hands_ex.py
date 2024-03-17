import cv2
import mediapipe as mp

# 미디어 파이프 손 모델 초기화
mp_hands = mp.solutions.hands
mp_drawing = mp.solutions.drawing_utils

# 손 인식을 위한 Hands 객체 생성
hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=1,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5)

# 웹캠에서 영상을 읽어옴
cap = cv2.VideoCapture(0)

if not cap.isOpened():
    print("웹캠을 열 수 없습니다.")
    exit()

while cap.isOpened():
    success, image = cap.read()
    if not success:
        print("영상을 읽는 데 실패했습니다.")
        break

    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    image.flags.writeable = False

    results = hands.process(image)

    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            thumb_tip = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP]
            index_finger_tip = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP]
            middle_finger_tip = hand_landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_TIP]

            # 엄지와 검지의 거리 계산
            thumb_index_distance = ((thumb_tip.x - index_finger_tip.x) ** 2 + (thumb_tip.y - index_finger_tip.y) ** 2) ** 0.5
            # 엄지와 중지의 거리 계산
            thumb_middle_distance = ((thumb_tip.x - middle_finger_tip.x) ** 2 + (thumb_tip.y - middle_finger_tip.y) ** 2) ** 0.5

            # 거리에 따른 조건 확인
            if thumb_index_distance < 0.1 and thumb_middle_distance < 0.1:  # 임계값은 실험을 통해 조정
                cv2.putText(image, "Cutting", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

            mp_drawing.draw_landmarks(
                image, hand_landmarks, mp_hands.HAND_CONNECTIONS)

    cv2.imshow('image', image)
    if cv2.waitKey(1) == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
