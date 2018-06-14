# PracticeLotsThings classes

## GameManager

### 描述
在此利用SubManager建立遊戲邏輯，是遊戲邏輯與引擎的唯一接口。

## abstract SubManager

### 描述
繼承這個抽象類別以自訂自己的遊戲邏輯，並在GameManager的Unity的Awake()或Start()中建立物件，利用Update()更新。

每個SubManager都必須對應一個Page，SubManager將會直接對Page進行操作。

### 靜態方法
- GetAllSubManagerNameInPage< T >：取得在指定Page中的SubManager名稱清單，主要用於Debug追蹤。

### 抽象方法
- Update()：將在GameManager的Update()內被呼叫，於引擎的每一禎執行一次。

## abstract Page

### 描述
繼承這個抽象類別以自訂自己的使用者介面，或任何顯示於遊戲中的物件。

## abstract InputDetecter

### 描述
偵測玩家輸入的偵測器，以PS4手柄為原型設計。它是一個抽像類別，藉由繼承它，可以自定自己的控制器邏輯。亦即遊戲內可以有多種輸入方式，但對需要接收輸入信號的物件來說，可以只以InputDetecter為接口，獲取須要的數值。

此外InputDetecter也屬於ScriptableObject，可以透過CreateAssetMenu屬性將自定的信號源打包成asset檔案存放在專案中。以目前存在的InputDetecter_JoyStick類別為例，可以透過Create>Controller>Joy Stick創建出Joy Stick的asset檔案，在其中自定輸入信號源供其他類別使用。

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

### 抽象方法
- DetectInput()：在這裡實做這個自訂控制器將信號源轉換成遊戲參數的邏輯。

## TimerManager

### 描述
計時器，用於需要延後觸發方法的時候。

### 靜態方法
- Schedulee(float time, Action action)：註冊一個延後時間觸發的方法，時間到時將執行註冊的方法，並反註冊。註冊時會回傳本次註冊的計時器ID。
- Tick(float deltaTime)：輸入減少的時間，每被呼叫一次，所有被註冊的計時器就會減少對應的時間。
- AddTime(lond id, float time)：輸入註冊時回傳的ID和時間，將會對對應ID的計時器作時間加減。

## ActorController

### 描述
用來控制角色模型的控制器。使用Page實做。

### 屬性
- ModelA：目前這個控制器正在使用的模型Animator。
- Direction_Vector：這個控制器目前正在行進的方向。
- Direction_MotionCurveValue：這個控制器目前的移動動態曲線。
- InputDetecter：這個控制器目前正在使用的輸入偵測器。
- CurrentAttackState：這個控制器目前的攻擊狀態。
- CurrentMoveState：這個控制器目前的移動狀態。
- IsGrounded：這個控制器目前是否正在地面上。
- IsLockOn：目前是否正鎖定在敵人身上。
- LockOnTarget：目前鎖定對象的Transform。

### 方法
- bool IsJumping()：取得這個控制器的角色是否正在跳躍中。
- bool IsIdle()：取得這個控制器是否正在待機狀態。

## CameraController

### 描述
攝影機控制器。根據指定的角色控制器給予的參數，設定攝影機的動態。

## AnimatorEventSender

### 描述
掛載於Animator State內，可以在Animator狀態機進入、離開、更新時傳送事件給所有註冊於其中，且符合tag的類別。

### 靜態方法
- RegisterOnStateEntered(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：註冊方法，在Animator進入擁有該tag的State時，執行它。
- RegisterOnStateExited(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：註冊方法，在Animator離開擁有該tag的State時，執行它。
- RegisterOnStateUpdated(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：註冊方法，在Animator更新擁有該tag的State時，執行它。
- UnregisterOnStateEntered(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：反註冊方法，令它在Animator進入擁有該tag的State時，不再執行它。
- UnregistOnStateExited(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：反註冊方法，令它在Animator離開擁有該tag的State時，不再執行它。
- UnregisterOnStateUpdated(string tag, ActorConroller actor, Action< AnimatorEventArgs > method)：反註冊方法，令它在Animator更新擁有該tag的State時，不再執行它。

### 方法
- override OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
https://docs.unity3d.com/ScriptReference/StateMachineBehaviour.OnStateEnter.html
- override OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
https://docs.unity3d.com/ScriptReference/StateMachineBehaviour.OnStateExit.html
- override OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
https://docs.unity3d.com/ScriptReference/StateMachineBehaviour.OnStateUpdate.html

## AnimationEventReceiver

### 描述
當動畫是利用FBX內的Animation時，由於其中的Animation Event是利用字串反射實做，故另外設計AnimationEventReceiver當作Animation Event和其他類別的接口，令Animation Event的字串固定為Invoke、InvokeInt、InvokeFloat、InvokeString、InvokeObject，減少發生人工失誤的可能。

當AnimationEventReceiver接收到Animation Event時，會發送執行所有註冊於其中的方法。

### 方法
- RegisterAction(Action action)：Action可以是Action、Action< int >、Action< float >、Action< string >、Action< object >，分別對應Invoke、InvokeInt、InvokeFloat、InvokeString、InvokeObject五種不同的動畫事件。利用這個方法註冊方法，當動畫事件被觸發時，執行它。
- UnregisterAction(Action action)：Action可以是Action、Action< int >、Action< float >、Action< string >、Action< object >，分別對應Invoke、InvokeInt、InvokeFloat、InvokeString、InvokeObject五種不同的動畫事件。利用這個方法反註冊方法，當動畫事件被觸發時，不再執行它。
- RegisterOnUpdatedRootMotion(Action< Vector3 > action)：註冊一個輸入Vector3的方法，當動畫的Root Motion更新時，執行它。
- UnregistOnUpdatedRootMotion(Action< Vector3 > action)：反註冊一個輸入Vector3的方法，當動畫的Root Motion更新時，不再執行它。

## HitBox

### 描述
掛載於需要當作HitBox的Game Object上即可。

### 靜態屬性
- HitBoxes：取得目前場景上所有啟動中的HitBox與對應的ActorController。
- OnOhitOthers：當HitBox擊中角色時將會發送這個事件。



