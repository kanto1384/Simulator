import pyautogui
import keyboard
import time

# 트리거 키 설정 (예: F8)
TRIGGER_KEY = 'F8'

# 더블클릭 후 이동할 Y축 거리 (양수면 아래로, 음수면 위로)
MOVE_Y = 100

print(f"[INFO] {TRIGGER_KEY} 키를 누르면 더블클릭 후 Y축 {MOVE_Y} 만큼 이동합니다.")
print("[INFO] 종료하려면 ESC를 누르세요.")

while True:
    # ESC로 종료
    if keyboard.is_pressed('esc'):
        print("[INFO] 종료합니다.")
        break

    # 트리거 키 입력 감지
    if keyboard.is_pressed(TRIGGER_KEY.lower()):
        # 현재 마우스 위치 저장
        x, y = pyautogui.position()

        # 더블클릭
        pyautogui.doubleClick(x, y)
        time.sleep(0.1)

        # 마우스 이동
        pyautogui.moveTo(x, y + MOVE_Y, duration=0.1)

        # 키 떼기 기다림 (중복 실행 방지)
        while keyboard.is_pressed(TRIGGER_KEY.lower()):
            time.sleep(0.05)
