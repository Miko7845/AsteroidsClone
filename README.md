# Asteroids Clone
Клон игры: [Arcade Game: Asteroids (1979 Atari)](https://www.youtube.com/watch?v=9Ydu8UhIjeU). 

Проект создан по тестовому заданию с определенными условями и ограничениями.  


### Астероиды
- [ ] Астероид имеет три состояния: Крупный, Средний, Малый.  
- [x] В начале уровня астероиды всегда крупные.  
- [x] При старте игры появляется 2 астероида.  
- [ ] После уничтожения всех астероидов - они появляются снова через 2 секунды, но на этот раз их на 1 астероид больше, чем до этого.  
- [ ] При появлении скорость выбирается случайно в определенных пределах.  
- [ ] При столкновении с пулей - Крупный или Средний астероид разламывается на 2 части, образуя астероиды поменьше.  
- [ ] Новые астероиды разлетаются в стороны, в направлении движения разрушенного астероида + 45 градусов и -45 градусов.
- [ ] Скорость новых астероидов: 1. Одинаковая. 2. Значение случайное. 3. Не больше, чем скорость уничтоженного астероида.  
- [x] При столкновении с игроком или НЛО - астероид полностью уничтожается, независимо от его размеров.  
  
### НЛО
- [ ] Появляется раз в 20-40 секунд с момента уничтожения последнего или начала игры.  
- [ ] Пролетает слева направо или справа налево (случайно), позиция по вертикали выбирается так же случайно, но не ближе чем 20% к границам экрана сверху или снизу.  
- [ ] Преодолевает экран примерно за 10 секунд. При столкновении с астероидом, НЛО уничтожается.  
- [ ] Во время перемещения стреляет по игроку со случайно частотой в пределах одного выстрела раз в 2-5 секунд. Пуля красного цвета. Скорость пули постоянная и такая же как у игрока.  
  
### Корабль игрока
- [x] Обладает следующими характеристиками: максимальная скорость, скорость поворота, ускорение.  
- [x] Движение корабля имеет инерцию.  
- [x] Трение отсутствует, поэтому скорость корабля не снижается.
- [x] Перемещение: корабль может только вращаться и ускорятся.
- [x] Игрок может стрелять, максимум 3 пули в сек. Для каждого выстрела, ему нужно нажать кнопку снова. Максимальное расстояние пуль = ширина экрана. После этого пуля исчезает.  
- [x] Корабль игрока появляется в центре экрана при старте игры или после уничтожения.  
- [x] После уничтожение скорость и другие динамические характеристики сбрасываются.  
- [ ] При спауне имеет неуязвимость в течении 3 секунд, полностью функционален в этот момент. Во время неуязвимости корабль появляется и исчезает с частотой 2 раза в секунду.  

### Мир и его границы
- [x] При выходе за пределы экрана пули, астероиды, игрок и другое появляются с противоположной стороны.  

### Интерфейс
**В игре:**
- [ ] На экране отображается кол-во жизней и очков.  
- [ ] Очки начисляются так: +20 за крупный астероид. +50 за средний. +100 за маленький. +200 за НЛО.
  
**В меню:**
- [ ] При входе в меню игра ставится на паузу.  
- [ ] Игровой интерфейс отображается (очки, кол-во жизней).  
- [ ] В меню расположены кнопки: Продолжить (недоступна, если игра не начата), Новая игра, “Управление: клавиатура” / “Управление: клавиатура + мышь” (меняет текст по клику, работает как переключатель), Выход.  
  
### Управление
- [ ] Есть две схемы управления (меняется в меню). 1. Клавиатура+Мышь. 2. Клавиатура.  
- [x] Управление: Стрельба - Пробел. Стрелки или WAD - поворот + ускорение. ESC - пауза, меню.  
  
### Требования к результату
-  Физику Unity можно использовать только для обнаружения столкновений. Перемещение астероидов, корабля, нло, пуль нужно написать самостоятельно.  
-  Нужно использовать стандартный Unity Input (Input.GetKeyDown и другие).  
-  Нужно реализовать object pool как минимум для пуль и астероидов. Реализацию пула напишите самостоятельно (не используйте UnityEngine.Pool).  
-  Нужно отоброзить в инспекторе те параметры игры, которые по вашему мнению понадобятся геймдизайнеру для настройки баланса игры.  
-  Без использования MVC, ECS, сотни интерфейсов и классов и другое сложное архитектурное.
-  Нельзя использовать сторонние ассеты, код готовых решений, фреймворков с github или других мест.

