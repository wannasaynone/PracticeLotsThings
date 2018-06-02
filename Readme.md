# PracticeLotsThings classes

## abstract InputDetecter

### 描述
偵測玩家輸入的偵測器，以PS4手柄為原型設計。它是一個抽像類別，藉由繼承它，可以自定自己的控制器邏輯。亦即遊戲內可以有多種輸入方式，但對需要接收輸入信號的物件來說，可以只以InputDetecter為接口，獲取須要的數值。

### 屬性
- LeftKey_Vertical：左控制桿的垂直動量。
- LeftKey_Horizontal：左控制桿的水平動量。
- RightKey_Vertical：右控制桿的垂直動量。
- RightKey_Horizontal：右控制桿的水平動量。
- KeyAPressed：A鍵是否按下過。
- KeyBPressed：B鍵是否按下過。
- KeyCPressed：C鍵是否按下過。
- KeyDPressed：D鍵是否按下過。
- KeyAPressing：A鍵是否正被按著。
- KeyBPressing：B鍵是否正被按著。
- KeyCPressing：C鍵是否正被按著。
- KeyDPressing：D鍵是否正被按著。

## ActorController

### 描述
用來控制角色模型的控制器。

### 屬性
- Model：目前這個控制器正在使用的模型GameObject。
- Direction_Vector：這個控制器目前正在行進的方向。
- Direction_MotionCurveValue：這個控制器目前的移動動態曲線。
- InputDetecter：這個控制器目前正在使用的輸入偵測器。
- CurrentAttackState：這個控制器目前的攻擊狀態。
- CurrentMoveState：這個控制器目前的移動狀態。
- IsGrounded：這個控制器目前是否正在地面上。

### 方法
- bool IsJumping()：取得這個控制器的角色是否正在跳躍中。
- bool IsIdle()：取得這個控制器是否正在待機狀態。

## CameraController

### 描述
攝影機控制器。根據指定的角色控制器給予的參數，設定攝影機的動態。

## CollisionDetector

### 描述
用來虛構一個碰撞體，並偵測這個碰撞體有無碰撞到指定Layer的物件。

### 靜態方法
- bool DectectCollision(CapsuleCollider capcaol, string layerName = "Default", Vector3 adjust = default(Vector3))
虛構出一個膠囊碰撞體，並回傳這個膠囊碰撞體有無碰撞到指令Layer的物件。

## AnimatorEventSender

### 描述
掛載於Animator State內，可以在Animator狀態機進入、離開、更新時傳送事件給所有註冊於其中，且符合tag的類別。

### 靜態方法
- RegistOnStateEntered(string tag, Action< AnimatorEventArgs > method)
- RegistOnStateExited(string tag, Action< AnimatorEventArgs > method)
- RegistOnStateUpdated(string tag, Action< AnimatorEventArgs > method)
- UnregistOnStateEntered(string tag, Action< AnimatorEventArgs > method)
- UnregistOnStateUpdated(string tag, Action< AnimatorEventArgs > method)
- UnregistOnStateExited(string tag, Action< AnimatorEventArgs > method)

### 方法
- OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
- OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
- OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)

## AnimationEventReceiver

### 描述
當動畫是利用FBX內的Animation時，由於其中的Animation Event是利用字串反射實做，故另外設計AnimationEventReceiver當作Animation Event和其他類別的接口，令Animation Event的字串固定為Invoke、InvokeInt、InvokeFloat、InvokeString、InvokeObject，減少發生人工失誤的可能。

當AnimationEventReceiver接收到Animation Event時，會發送執行所有註冊於其中的方法。

### 方法
- RegistAction(Action action)
- UnregistAction(Action action)
- RegistOnUpdatedRootMotion(Action< Vector3 > action)
- UnregistOnUpdatedRootMotion(Action< Vector3 > action)
