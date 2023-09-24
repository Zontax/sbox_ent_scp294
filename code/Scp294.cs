using System.Buffers;
using Sandbox;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Bimbasic;

[Library
("ent_scp_294", Title = "SCP-294", Description =
"Item SCP-294 appears to be a standard coffee vending machine, the only noticeable difference being an entry touchpad with buttons corresponding to an English QWERTY keyboard. Upon depositing fifty cents US currency into the coin slot, the user is prompted to enter the name of any liquid using the touchpad. Upon doing so, a standard 12-ounce paper drinking cup is placed and the liquid indicated is poured.")]
public partial class Scp294 : ModelEntity, IUse
{
    Keyboard clientKeyboard;
    DrinkInfoPanel panel;
    Particles particle;
    [Net, Change] public string FindingName { get; set; } = "water";
    [Net, Predicted] int drinkIndex { get; set; } = 0;
    [Net, Predicted] TimeSince useReloadMillis { get; set; }
    [Net, Predicted] Sound sound_use { get; set; }
    [Net, Predicted] bool isTouched { get; set; }
    [Net, Predicted] bool isUse { get; set; }
    [Net, Predicted] bool isOutOfRange { get; set; }
    Vector3 offset = new(-17.05f, 11.3f, 32.5f);
    Vector3 offsetDrinkPanel = new(-20.485f, -11.7f, 62f);
    float useReload = 0.5f;
    float spawnTime;

    public List<string> drinks = new()
    {
        @"
        [Water|H2O]
        ru[Вода|Водичка]
        uk[Вода]
        color=50,120,255
        alpha=0.35
        message={Well, that was refreshing.}
        messageRu={Что ж, это было освежающе.}
        messageUk={Що ж, це було освіжаюче.}
        sound={ahh}
        ",
        @"
        [Air|Oxygen|Nothing|Cup|Vacuum|Void|Emptiness|Nope|Null|Zero|HL3|Half-Life 3|Half Life 3]
        ru[Воздух|Кислород|Ничего|Чашка|Вакуум|Пустота]
        uk[Повітря|Кисень|Нічого]
        color=255,255,255
        alpha=0.6
        message={There is nothing to drink in the cup.}
        refuse=true
        refuseMessage={There is nothing to drink in the cup.}
        refuseMessageRu={В стакане нечего пить.}
        refuseMessageUk={В стакані нічого пити.}
        sound={none}
        dispenseSound={dispense0}
        ",
        @"
        [Juice|Orange Juice|Orange]
        ru[Сок|Апельсиновый сок|Апельсин|Оранжевый|Оранжевое]
        uk[Сік|Апельсиновий сік|Помаранчевий]
        color=240,175,70
        alpha=0.8
        message={Just fresh orange juice.}
        messageRu={Просто свежий апельсиновый сок.}
        messageUk={Просто свіжий апельсиновий сік.}
        sound={ahh}
        ",
        @"
        [Tomato Juice|Tomato|Tomat]
        ru[Томатный сок|Помидор]
        uk[Томатний сік|Помідор]
        color=204,0,0
        alpha=0.9
        message={Meh, it's ok. Never was a big fan of tomato juice though.}
        messageRu={Все в порядке. Хотя я не большой поклонник томатного сока.}
        messageUk={Все в порядку. Хоча я не великий пошановувач томатного сока.}
        sound={ahh}
        ",
        @"
        [Beer|Lager|Pale Beer|Light Beer]
        ru[Пиво|Светлое пиво|Пивко]
        uk[Пиво|Світле пиво]
        color=235,165,40
        alpha=0.8
        message={To taste the usual light beer, cold.}
        messageRu={По вкусу обычное светлое пиво, холодное.}
        messageUk={За смаком звичайне світле пиво, холодне.}
        sound={ahh}
        ",
        @"
        [Coffee|Black Coffee|Coffe|Black Coffee|Caffe]
        ru[Кофе|Чорный кофе]
        uk[Кава|Чорна кава]
        color=87,62,45
        message={The drink tastes like fairly strong black coffee.}
        ",
        @"
        [Espresso]
        ru[Эспрессо]
        uk[Еспрессо]
        color=95,65,47
        message={The drink tastes like fairly tasty espresso.}
        messageRu={По вкусу напиток напоминает довольно вкусный эспрессо.}
        messageUk={По смаку напій нагадує досить смачний еспрессо.}
        ",
        @"
        [Americano]
        ru[Американо]
        uk[Американо]
        color=90,60,45
        message={The drink tastes like fairly strong americano.}
        ",
        @"
        [Latte|Caffe latte]
        ru[Латте]
        uk[Лате]
        color=250,200,250
        message={The drink tastes like latte.}
        ",
        @"
        [Pumpkin latte]
        ru[Тыквенный латте]
        uk[Гарбузовий лате]
        color=255,136,0
        message={The drink tastes like pumpkin latte.}
        ",
        @"
        [Alcohol|Ethanol|Spirit|Vodka|Horilka]
        ru[Алкоголь|Этанол|Жидкий этанол|Водка]
        uk[Алкоголь|Етанол|Рідкий етанол|Горілка]
        color=230,230,230
        alpha=0.7
        message={Damn, that's strong...}
        messageRu={Блин, это сильно...}
        messageUk={Блін, це сильно...}
        sound={ew1}
        ",
        @"
        [Uranium|Uranus]
        ru[Уран|Уран-238|Уран 238]
        uk[Уран]
        color=70,170,90
        alpha=1
        glow=true
        message={Uranium so bad.} 
        lethal=true
        deathTimer=2
        deathMessage={Subject ##### was found in the facility cafeteria lying in a small puddle of depleted uranium. Cause of death was confirmed to be acute radioactive poisoning.}
        damage=90
        sound={burn}
        dispenseSound={dispense3}
        ",
        @"
        [Bleach]
        ru[Отбеливатель]
        uk[Відбілювач]
        color=255,255,250
        message={The liquid burns in your mouth and throat.} 
        lethal=true
        deathTimer=5
        deathMessage={The liquid burns in your mouth and throat. You dead.}
        damage=90
        sound={burn}
        dispenseSound={dispense2}
        ",
        @"
        [Mucus]
        ru[Слизь|Cлизи]
        uk[Слиз|Слизу]
        color=102,204,0
        alpha=0.9
        refuse=true
        refusemessage={Why the hell would I want to drink this?!?}
        dispenseSound={dispense2}
        ",
        @"
        [Nausea]
        ru[Тошнота]
        uk[Нудота]
        color=59,92,32
        alpha=0.6
        refuse=true
        refusemessage={Why the hell would I want to drink this?!?}
        vomit=true
        stomachAche=true
        sound={slurp}
        dispenseSound={dispense3}
        ",
        @"
        [Tea|Black tea]
        ru[Чай|Черный Чай|Чая|Чаек]
        uk[Чай|Чорний чай|Чаю|Чайок]
        color=65,20,0
        alpha=0.6
        glow=true
        message={Tasty black tea.}
        messageRu={Вкусный чёрный чай.}
        messageUk={Смачний чорний чай.}
        sound={slurp1}
        ",
        @"
        [Green tea]
        ru[Зеленый Чай|Зелёный чай]
        uk[Зелений Чай]
        color=70,200,0
        alpha=0.6
        glow=true
        message={Tasty black tea.}
        messageRu={Вкусный чёрный чай.}
        messageUk={Смачний чорний чай.}
        sound={slurp1}
        ",
        @"
        [Coca-cola|Coke|Cola]
        ru[Кока-кола|Кола]
        uk[Кока-кола]
        color=65,20,0
        message={Just Coca-cola.}
        messageRu={Просто Кока-кола.}
        messageUk={Просто Кока-кола.}
        ",
        @"
        [Coca-cola zero|Coke zero|Cola zero]
        ru[Coca-cola zero|Диетическая кока-кола|Кола без сахара]
        uk[Coca-cola zero]
        color=68,30,0
        message={Tastes like cola zero without sugar.}
        messageRu={На вкус как кола zero без сахара.}
        messageUk={На смак як кола zero без цукру.}
        ",
        @"
        [Pepsi|Pepsi-cola]
        ru[Пепси|Пепси-кола]
        uk[Пепсі|Пепсі-кола]
        color=65,25,0
        message={Just Pepsi-cola.}
        messageRu={Просто пепси-кола.}
        messageUk={Просто пепсі-кола.}
        sound={ahh}
        ",
        @"
        [Fanta|Orange Fanta]
        ru[Фанта, Апельсиновая фанта|Фанта со вкусом апельсина]
        uk[Фанта, Апельсинова фанта|Фанта зі смаком апельсина]
        color=240,175,70
        alpha=0.8
        message={Just orange Fanta.}
        sound={ahh}
        ",
        @"
        [Sprite]
        ru[Спрайт]
        uk[Спрайт]
        color=200,225,200
        alpha=0.6
        message={Just Sprite.}
        sound={ahh}
        ",
        @"
        [Lemonade]
        ru[Лимонад]
        uk[Лимонад]
        color=245,245,130
        alpha=0.8
        message={The drink tastes sweet and tart.}
        messageRu={Напиток сладкий и терпкий.}
        messageUk={Напій слолодкий та терпкий.}
        sound={ahh}
        ",
        @"
        [Energy Drink]
        ru[Энергетический напиток|Энергетик]
        uk[Енергетичний напій|Енергетик]
        color=50,215,70
        alpha=0.9
        message={It tastes slightly strong and tart.}
        blur=5
        sound={ahh}
        ",
        @"
        [Carbon|Liquid Carbon]
        ru[Карбон|Жидкий карбон]
        uk[Карбон|карбон]
        color=0,0,0
        alpha=0.9
        glow=true
        message={The mouth cavity, throat and face of the victim are covered in severe third degree burns. A layer of black, crystalline substance was found inside the mouth cavity. Sent for chemical analysis.}
        stomachAche=true
        blur=10
        damage=30
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Wine|Red Wine|Grape Wine]
        ru[Вино|Красное вино|Виноградное вино]
        uk[Вино|Червоне вино|Виноградне вино]
        color=145,32,50
        alpha=0.9
        message={It tastes like good wine.}
        messageRu={На вкус как хорошее вино.}
        messageUk={На смак як добре вино.}
        blur=5
        ",
        @"
        [Blood of Christ|Blood of Jesus|Blood of Jesus Christ]
        ru[Кровь Христа|Кровь Иисуса|Кровь Иисуса Христа]
        uk[Кров Христа|Кров Ісуса|Кров Ісуса Христа]
        color=200,20,10
        alpha=0.9
        message={The drink tastes like red wine.}
        messageRu={По вкусу напиток напоминает красное вино.}
        messageUk={По смаку напій нагадує червоне вино.}
        sound={ahh}
        dispenseSound={dispense3}
        ",
        @"
        [Blood|Someones blood|Whose blood|Dead|Human|Man|Woman|Baby|Child|Cat|Dog|Animal]
        ru[Кровь|Чьята кровь|Ребeнок|Кот|Собака|Пёс|Животное]
        uk[Кров|Чиясь кров|Дитина|Кіт|Собака|Пес|Тварина]
        color=200,20,10
        message={It really is someone's blood.}
        messageRu={Это похоже чья-то кровь.}
        messageUk={Це похоже чиясь кров.}
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Alien blood|Alien]
        ru[Кровь чужого|Кровь пришельца]
        uk[Кров чужого|Кров прибульця]
        color=50,145,60
        alpha=0.9
        message={Alien blood?}
        messageRu={Кровь пришельца?}
        messageUk={Кров прибульця?}
        blur=10
        sound={bee}
        dispenseSound={dispense3}
        ",
        @"
        [Leukemia]
        ru[Лейкемия|Лейкемии]
        uk[Лейкемія|Лейкемії]
        color=145,20,60
        message={Looks like you have leukemia.}
        messageRu={Похоже вы заболели лейкимией.}
        messageUk={Схоже ви захворіли лейкімією.}
        blur=10
        sound={bee}
        dispenseSound={dispense3}
        ",
        @"
        [Glass]
        ru[Стекло]
        uk[Скло]
        color=255,0,0
        alpha=0.9
        message={You felt a tingling sensation in your throat.}
        lethal=true
        deathTimer=15
        deathMessage={You felt a tingling sensation in your throat.}
        stomachAche=true
        blur=10
        damage=90
        vomit=true
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Gold|Golden Drink]
        ru[Золото|Золотой напиток]
        uk[Золото|Золотий напій]
        color=255,200,15
        glow=true
        message={You felt a tingling sensation in your throat.}
        lethal=true
        deathTimer=2
        deathMessage={You felt a tingling sensation in your throat.}
        damage=80
        sound={burn}
        dispenseSound={dispense2}
        ",
        @"
        [Iron|Steel|Metal|Lava]
        ru[Железо|Сталь|Металл]
        uk[Залізо|Сталь|Метал]
        color=255,100,30
        glow=true
        lethal=true
        deathTimer=2
        deathMessage={You felt a tingling sensation in your throat.}
        damage=80
        sound={burn}
        dispenseSound={dispense2}
        ",
        @"
        [Nitrogen|Liquid Nitrogen]
        ru[Азот|Жидкий азот]
        uk[Азот|Рідкий азот]
        color=255,255,255
        alpha=0.5
        lethal=true
        deathMessage={Eat nitrogen, genius...}
        stomachAche=true
        blur=10
        damage=30
        vomit=true
        sound={slurp}
        dispenseSound={dispense2}
        ",
        @"
        [Egg|Eggs]
        ru[Яйцо|Яйца]
        uk[Яйце|Яйця]
        color=252,252,212
        message={It tastes just like raw eggs!}
        messageRu={По вкусу как сырые яйца!}
        messageUk={На смак як сирі яйця!}
        dispenseSound={dispense2}
        ",
        @"
        [Milk]
        ru[Молоко|Молочко]
        uk[Молоко]
        color=255,255,255
        alpha=0.9
        message={This is tasty milk.}
        messageRu={Это вкусное молоко.}
        messageUk={Це смачне молоко.}
        ",
        @"
        [Chocolate Milk]
        ru[Шоколадное молоко]
        uk[Шоколадне молоко]
        color=150,50,0
        alpha=0.6
        message={Just chokolate milk.}
        ",
        @"
        [Room-temperature Superconductor]
        ru[Сверхпроводник при комнатной температуре]
        uk[Надпровідник при кімнатній температурі|Надпровідник за кімнатної температури]
        color=254,219,93
        alpha=0.5
        message={For some reason, the drink tastes like apple juice.}
        messageRu={Почему-то напиток имеет вкус яблочного сока.}
        messageUk={Чомусь напій має смак яблучного соку.}
        dispenseSound={dispense3}
        ",
        @"
        [Feces|Fecal matter|Shit|Crap|Poo|Poop|Dung|Scat|Turd|Bullshit|Horseshit|Diarrhea]
        ru[Фекалии|Фекалия|Дерьмо|Говно|Гной|Какашка|Какашки|Навоз|Херня|Диарея]
        uk[Фекалії|Фекалія|Лайно|Гівно|Гній|Хєрня|Діарея]
        color=87,62,45
        alpha=0.9
        message={Я не буду это пить оно выглядит и пахнет отвратительно.}
        refuse=true
        refuseMessage={I'm not going to drink it, it looks and smells disgusting.}
        sound={none}
        dispenseSound={dispense2}
        ",
        @"
        [Urine|Piss|Pee]
        ru[Моча]
        uk[Сеча]
        color=240,240,0
        alpha=0.9
        message={It's disgusting!}
        messageRu={Это отвратительно!}
        messageUk={Це огидно!}
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Fear|Scare|Horror|Terror]
        ru[Страх|Испуг|Ужас|Террор]
        uk[Страх|Перепуг|Жах|Террор]
        color=32,0,0
        lethal=true
        deathTimer=5
        deathMessage={You died of terror....}
        deathMessageRu={Вы умерли от ужаса...}
        deathMessageUk={Ви померли від жаху...}
        blur=20
        sound={ew1}
        dispenseSound={dispense3}
        ",
        @"
        [Death|Game Over|Kill]
        ru[Смерть]
        uk[Смерть]
        color=15,0,0
        lethal=true
        deathMessage={You died instantly...}
        deathMessageRu={Вы умерли мгновенно...}
        deathMessageUk={Ви померли миттєво...}
        blur=20
        sound={slurp1}
        dispenseSound={dispense3}
        ",
        @"
        [Honey]
        ru[Сладость|Сладости|Мёд]
        uk[Солодощі|Насолода|Мед]
        color=224,198,79
        alpha=0.8
        message={Mmm... sweet.}
        messageRu={Ммм... сладко.}
        messageUk={Ммм... солодко.}
        dispenseSound={dispense2}
        ",
        @"
        [Courage|Bravery]
        ru[Храбрость|Отвага]
        uk[Сміливість|Відвага]
        color=41,28,22
        message={You are suddenly overcome with a feeling of confidence!}
        messageRu={Вас внезапно охватывает чувство уверенности!}
        messageUk={Вас раптово охоплює почуття сміливості!}
        dispenseSound={dispense3}
        ",
        @"
        [Luck]
        ru[Удача]
        uk[Вдача]
        color=240,240,128
        glow=true
        refuse=true
        refuseMessage={When you reach for the cup, you clumsily knock it to the ground and the contents spill all over the floor. How ironic.}
        sound={none}
        dispenseSound={dispense3}
        ",
        @"
        [Acid|Acid Liquid|Liquid Acid|Poison]
        ru[Кислота|Кислотная смесь]
        uk[Кислота|Кислотний напій]
        color=10,90,10
        glow=true
        message={Acid.}
        damage=30
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Motor Oil|Oil]
        ru[Моторное масло|Масло|Моторная жидкость]
        uk[Моторне масло|Моторне мастило|Мастило|Моторна рідина]
        color=0,0,0
        alpha=1
        message={It's motor oil, it's not tasty.}
        messageRu={Это моторное масло, оно невкусное.}
        messageUk={Це моторне масло, воно несмачне.}
        blur=10
        damage=50
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [The best drink I've ever had|The best drink I've had|The best drink Ive ever had|The best drink Ive had]
        ru[Лучший напиток который я когда-либо пил|Лучший напиток который я пил|Лучший напиток что я когда-либо пил|Лучший напиток что я пил]
        uk[Найкращий напій який я коли-небудь пив|Найкращий напій який я пив|Найкращий напій що я коли-небудь пив|Найкращий напій що я пив|Найкращий напій з усіх які я будь-коли пив]
        color=235,165,40
        alpha=0.8
        message={The drink tastes like a Vienna lager you drinked years ago.}
        messageRu={Напиток по вкусу напоминает венский лагер, который вы пили много лет назад.}
        messageUk={Напій на смак нагадує віденський жегер, який ви пили багато років тому.}
        sound={ahh}
        dispenseSound={dispense3}
        ",
        @"
        [I|Me|You|Myself|Yourself|Himself]
        ru[Я|Меня|Моя кровь|Напиток из меня]
        uk[Я|Мене|Моя кров|Напій з мене]
        color=235,40,40
        alpha=0.8
        message={Вам стало дуже погано.}
        messageRu={Вам стало очень плохо.}
        messageUk={Вам стало дуже погано.}
        damage=60
        sound={bee}
        dispenseSound={dispense3}
        ",
        @"
        [Aloe Vera Drink|Cactus Drink|Aloe Vera|Cactus]
        ru[Алое Вера|Алое|Кактус]
        uk[Алоє Вєра|Алоє|Кактус]
        color=217,214,186
        alpha=0.8
        blur=10
        heal=true
        sound={ohh}
        dispenseSound={dispense1}
        ",
        @"
        [Anti-Energy Drink|Anti Energy Drink|Tired]
        ru[Анти енергетик|Анти енергетический напиток|Утомление]
        uk[Анти енергетик|Анти енергетичний напій|Втома]
        color=250,180,12
        alpha=1
        message={The drink tastes terrible. You feel tired and drained.}
        messageRu={У напитка ужасный вкус. Вы чувствуете усталость и истощение.}
        messageUk={У напоя жахливий смак. Ви відчуваєте втому та виснаження.}
        damage=25
        sound={bee}
        dispenseSound={dispense3}
        ",
        @"
        [Antimatter|Anti-matter|Negative Matter|Higgs Boson|Exotic Matter|Zero Point Energy|Gravitons|God Particles|Black Holes]
        ru[Антиматерия|Анти-материя|Античастички|Бозон Хиггса]
        uk[Антиматерія|Анти-матерія|Античастинки|Бозон Хіггса]
        color=0,0,40
        alpha=0.5
        deathMessage={Recon teams sent in at [REDACTED] show that everything within a 210 mile radius from [REDACTED] was vaporized, save for a 5 meter radius of unharmed area in the [REDACTED], surrounding SCP-294}
        deathMessageRu={Группы, отправленные в [УДАЛЕНО], показывают, что все в радиусе 210 миль от Зоны-[УДАЛЕНО] испарилось, за исключением 5-метрового радиуса неповрежденной зоны, окружающей SCP-294.}
        deathMessageUk={Групи, відправлені в [ВИДАЛЕНО], показують, що все в радіусі 210 миль від [ВИДАЛЕНО] випарувалось, за виключенням 5-метрового радіуса непошкодженої зони, що оточує SCP-294.}
        explosion=true
        explosionTimer=1
        damage=9999
        dispenseSound={dispense3}
        ",
        @"
        [Bomb|Explosive|Explosives|Semtex|Grenade]
        ru[Бомба|Взрывчатка|Взрыв|Бомбочка|Взрывоопасное]
        uk[Бомба|Вибухівка|Вибух|Вибухонебезпечне|Бімба]
        color=11,12,13
        message={It is logical that the bomb explodes...}
        messageRu={Бомба делает бум.}
        messageUk={Бомба робить бум.}
        deathMessage={Shit...}
        explosion=true
        dispenseSound={dispense3}
        ",
        @"
        [Surprise me|Surprise]
        ru[Удиви меня]
        uk[Здивуй мене]
        color=11,12,250
        message={Непрозрачный стаканчик, содержавший нормальную с виду воду, которая, как позже было установлено, на самом деле была нагрета примерно до 200°С. Когда стаканчик был взят в руки, от вибрации его содержимое превратилось в пар, разбрызгивая кипящую воду в 2-метровом радиусе.}
        messageRu={Непрозрачный стаканчик, содержавший нормальную с виду воду, которая, как позже было установлено, на самом деле была нагрета примерно до 200°С. Когда стаканчик был взят в руки, от вибрации его содержимое превратилось в пар, разбрызгивая кипящую воду в 2-метровом радиусе.}
        messageUk={Непрозорий стаканчик, що містив нормальну на вигляд воду, яка, як пізніше було встановлено, насправді була нагріта приблизно до 200°С. Коли стаканчик було взято до рук, від вібрації його вміст перетворився на пару, розбризкуючи киплячу воду в 2-метровому радіусі.}
        lethal=false
        deathTimer=10
        deathMessage={Shit...}
        explosion=true
        damage=1
        dispenseSound={dispense3}
        ",
        @"
        [Cup of Music|Music]
        ru[Чашка музыки|Музыка|Музыки]
        uk[Чашка музики|Музика|Музики]
        color=0,0,60
        alpha=0.85
        message={You feel the music.}
        messageRu={Вы почувствовали музыку.}
        messageUk={Ви відчули музику.}
        sound={cup_of_music}
        dispenseSound={dispense3}
        ",
        @"
        [Perfect drink]
        ru[Идеальный напиток]
        uk[Ідеальний напій]
        color=230,230,250
        message={You are in shock, after this drink, everything else seems disgusting, you have lost the meaning of your life...}
        messageRu={Вы в шоке, после этого напитка, все остальное кажется гадостью, вы потеряли смысл своей жизни...}
        messageUk={Ви в шоці, після цього напою, все інше видається гидотою, ви втратили сенс свого життя...}
        damage=1
        sound={ahh}
        dispenseSound={dispense3}
        ",
        @"
        [Vanilla Cake Batter]
        ru[Ванильное тесто для торта]
        uk[Ванільне тісто для торта]
        color=230,215,185
        alpha=0.9
        message={The drink tastes delicious, albeit sweet.}
        sound={slurp}
        dispenseSound={dispense3}
        ",
        @"
        [Wax]
        ru[Воск]
        uk[Віск]
        color=203,203,199
        message={The taste makes you ill.}
        messageRu={От вкуса вам становится плохо.}
        messageUk={Від смаку вам стає погано.}
        stomachAche=true
        sound={slurp}
        dispenseSound={dispense2}
        ",
        @"
        [Syrup of ipecac]
        ru[Сироп ипекакуаны]
        uk[Сироп іпекакуани]
        color=106,89,30
        message={You feel very sick.}
        explosion=false
        stomachAche=true
        blur=5
        damage=5
        sound={slurp}
        dispenseSound={dispense2}
        ",
        @"
        [My life story]
        ru[История моей жизни]
        uk[Історія мого життя]
        color=106,89,30
        message={You feel a little bad, but you remembered the whole story of your life.}
        message={Вам немного плохо, но вы вспомнили всю историю своей жизни.}
        message={Вам трохи погано, але ви згадали всю історію свого життя.}
        blur=5
        sound={slurp}
        dispenseSound={dispense2}
        ",
        @"
        [Wood|Chipped wood]
        ru[Дерево]
        uk[Дерево]
        color=170,135,88
        message={I'm going to throw up! It's chipped wood.}
        stomachAche=true
        damage=1
        sound={ew2}
        dispenseSound={dispense2}
        ",
        @"
        [LSD|Lysergic acid diethylamide]
        ru[ЛСД|диэтиламид лизергиновой кислоты]
        uk[ЛСД|діетиламід лізергінової кислоти]
        color=255,0,255
        glow=true
        message={HOLY CRAP EVERTHING IS MULTICOLORED}
        lethal=true
        deathTimer=10
        deathMessage={Subject [DELETED] was found in the cafeteria, in a psychopatic state.}
        sound={ahh}
        dispenseSound={dispense2}
        ",
        @"
        [Cum|Sperm|Semen|Seed|Ejaculate|Spermatozoa|Spermatozoon|Sex]
        ru[Сперма|Эякулят|Сперматозоид|Секс]
        uk[Сперма|Еякулят|Сперматозоїд]
        color=255,250,250
        message={Semen.}
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Test228]
        ru[Тест228]
        uk[Тест288]
        color=0,0,0
        alpha=0.9
        glow=false
        message={}
        lethal=false
        deathTimer=0
        deathMessage={}
        deathMessageRu={}
        deathMessageUk={}
        explosion=false
        stomachAche=false
        blur=5
        refuse=false
        refuseMessage={}
        heal=false
        damage=5
        godmode=false
        vomit=false
        sound={slurp}
        dispenseSound={dispense3}
        ",
    };

    string[] drinkNames = { "Drink" };
    string[] drinkNamesRu = { "Drink" };
    string[] drinkNamesUk = { "Drink" };
    int r, g, b = 255; float alpha = 1;
    bool glow = false;
    string message = "Tasty drink.";
    string messageRu = "Вкусный напиток.";
    string messageUk = "Смачний напій.";
    bool lethal = false;
    float deathTime = 0;
    string deathMessage = "";
    string deathMessageRu = "";
    string deathMessageUk = "";
    bool explosion = false;
    float explosionTime = 0;
    bool stomachAche = false;
    int blur = 0;
    bool refuse = false;
    string refuseMessage = "I don't wont it drink";
    string refuseMessageRu = "Я не хочу это пить";
    string refuseMessageUk = "Я не хочу це пити";
    bool heal = false;
    int damage = 0;
    bool godmode = false;
    bool vomit = false;
    string sound = "slurp";
    string dispenseSound = "dispense1";

    void OnFindingNameChanged(string oldValue, string newValue)
    {
        Log.Info($"findingName changed [{oldValue}], now it is [{newValue}]");
    }

    public override void Spawn()
    {
        base.Spawn();
        PhysicsEnabled = true;
        UsePhysicsCollision = true;
        EnableTouchPersists = true;
        SetModel("models/scp_294/scp_294_rotated.vmdl");
        //Model = Cloud.Model("bimbasic.scp294");
        SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
        //this.Rotation = this.Rotation * Rotation.From(0, 180f, 0);
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();
        SpawnPanel();
    }

    [ClientRpc]
    public void SpawnPanel(string name = "")
    {
        panel = new();
        panel.scp294 = this;
        panel.Position = this.Position + (offsetDrinkPanel * this.Rotation);
        panel.Rotation = this.Rotation * Rotation.From(0f, 180f, 0f);
        panel.SetLabel(name);
    }

    [ClientRpc]
    public void DeletePanel()
    {
        panel.Delete(true);
    }

    [ClientRpc]
    protected void AddKeyboard()
    {
        Keyboard[] keys = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.ChildrenOfType<Keyboard>().ToArray();

        if (keys.Count() == 0)
        {
            clientKeyboard = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.AddChild<Keyboard>();
            clientKeyboard.scp = this;
        }
    }

    [ClientRpc]
    public void DeleteKeyboard()
    {
        clientKeyboard.Delete();
    }

    public override void StartTouch(Entity other)
    {
        if (other is Cup cup)
        {
            isTouched = true;
        }
    }

    public override void EndTouch(Entity other)
    {
        if (other is Cup cup)
        {
            isTouched = false;
        }
    }

    int FindDrinkInList(string name)
    {
        if (name == "random" || name == "random drink")
        {
            int randIndex = Game.Random.Next(0, drinks.Count());
            return randIndex;
        }

        if (name == "repeat" || name == "late returns" || name == "last choice" || name == "last" || name == "повтор")
        {
            return FindDrinkInList(drinkNames[0]);
        }

        foreach (string drink in drinks)
        {
            string[] lines = drink.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string pattern = @"\[([^\]]+)\]";
                MatchCollection matches = Regex.Matches(line, pattern);

                foreach (Match match in matches)
                {
                    string fullName = match.Groups[1].Value;
                    string[] nameParts = fullName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string part in nameParts)
                    {
                        if (part.Trim().Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            int index = drinks.IndexOf(drink);
                            return index;
                        }
                    }
                }
            }
        }
        return -1;
    }

    void ParseDrinkProperties(int index)
    {
        if (index < 0 || index >= drinks.Count) return;
        T GetValueOrDefault<T>(Group group, T defaultValue = default)
        {
            if (group.Success)
            {
                try
                {
                    return (T)Convert.ChangeType(group.Value, typeof(T));
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        bool TryParseBool(Group group, bool defaultValue)
        {
            return group.Success && bool.TryParse(group.Value, out bool result) ? result : defaultValue;
        }

        string pattern = @"\[([^\]]+)\]\s*ru(\[[^\]]+\])?\s*uk(\[[^\]]+\])?(\s*color=(\d+),(\d+),(\d+))?\s*(alpha=([\d.]+))?\s*(glow=(true|false))?\s*(message=\{(.*?)\})?\s*(messageRu=\{(.*?)\})?\s*(messageUk=\{(.*?)\})?\s*(lethal=(true|false))?\s*(deathTimer=([\d.]+))?\s*(deathMessage=\{(.*?)\})?\s*(deathMessageRu=\{(.*?)\})?\s*(deathMessageUk=\{(.*?)\})?\s*(explosion=(true|false))?\s*(explosionTimer=([\d.]+))?\s*(stomachAche=(true|false))?\s*(blur=([\d.]+))?\s*(refuse=(true|false))?\s*(refuseMessage=\{(.*?)\})?\s*(refuseMessageRu=\{(.*?)\})?\s*(refuseMessageUk=\{(.*?)\})?\s*(heal=(true|false))?\s*(damage=([\d.]+))?\s*(godmode=(true|false))?\s*(vomit=(true|false))?\s*(sound=\{([^\}]*)\}\s*)?\s*(dispenseSound=\{([^\}]*)\}\s*)?";

        Match match = Regex.Match(drinks[index], pattern, RegexOptions.Singleline);
        if (match.Success)
        {
            drinkNames = match.Groups[1].Value.Replace("]", "").Replace("[", "").Split('|');
            drinkNamesRu = match.Groups[2].Value.Replace("]", "").Replace("[", "").Split('|');
            drinkNamesUk = match.Groups[3].Value.Replace("]", "").Replace("[", "").Split('|');
            r = GetValueOrDefault(match.Groups[5], 255);
            g = GetValueOrDefault(match.Groups[6], 255);
            b = GetValueOrDefault(match.Groups[7], 255);
            alpha = GetValueOrDefault(match.Groups[9], 1);
            glow = TryParseBool(match.Groups[11], false);
            message = GetValueOrDefault(match.Groups[13], "");
            messageRu = GetValueOrDefault(match.Groups[15], message);
            messageUk = GetValueOrDefault(match.Groups[17], message);
            lethal = TryParseBool(match.Groups[19], false);
            deathTime = GetValueOrDefault(match.Groups[21], 0);
            deathMessage = GetValueOrDefault(match.Groups[23], "");
            deathMessageRu = GetValueOrDefault(match.Groups[25], deathMessage);
            deathMessageUk = GetValueOrDefault(match.Groups[27], deathMessage);
            explosion = TryParseBool(match.Groups[29], false);
            explosionTime = GetValueOrDefault(match.Groups[31], 0);
            stomachAche = TryParseBool(match.Groups[33], false);
            blur = GetValueOrDefault(match.Groups[35], 0);
            refuse = TryParseBool(match.Groups[37], false);
            refuseMessage = GetValueOrDefault(match.Groups[39], "");
            refuseMessageRu = GetValueOrDefault(match.Groups[41], "");
            refuseMessageUk = GetValueOrDefault(match.Groups[43], "");
            heal = TryParseBool(match.Groups[45], false);
            damage = GetValueOrDefault(match.Groups[47], 0);
            godmode = TryParseBool(match.Groups[49], false);
            vomit = TryParseBool(match.Groups[51], false);
            sound = GetValueOrDefault(match.Groups[53], "slurp");
            dispenseSound = GetValueOrDefault(match.Groups[55], "dispense1");
        }
    }

    void SpawnParticle()
    {
        //var particle = Particles.Create("particles/dispence.vpcf", this, "effect", true);
        //particle.SetPosition(0, new Vector3(100, 200, 100));

        particle = Particles.Create(Cloud.ParticleSystem("penguins/water").ResourcePath);
        particle.SetPosition(0, this.Position + (offset * this.Rotation));
        //Cloud.ParticleSystem("penguins/water");
    }

    void SpawnDrink()
    {
        Drink drink = new()
        {
            Position = this.Position + (offset * this.Rotation),
            Rotation = this.Rotation,

            name = this.drinkNames[0],
            nameRu = this.drinkNamesRu[0],
            nameUk = this.drinkNamesUk[0],
            RenderColor = Color.FromBytes(r, g, b, Map((int)(alpha * 1000), 0, 1000, 0, 255)),
            r = this.r,
            g = this.g,
            b = this.b,
            alpha = this.alpha,
            glow = this.glow,
            message = this.message,
            messageRu = this.messageRu,
            messageUk = this.messageUk,
            lethal = this.lethal,
            deathTime = this.deathTime,
            deathMessage = this.deathMessage,
            deathMessageRu = this.deathMessageRu,
            deathMessageUk = this.deathMessageUk,
            explosion = this.explosion,
            explosionTime = this.explosionTime,
            stomachAche = this.stomachAche,
            blur = this.blur,
            refuse = this.refuse,
            refuseMessage = this.refuseMessage,
            refuseMessageRu = this.refuseMessage,
            refuseMessageUk = this.refuseMessage,
            heal = this.heal,
            damage = this.damage,
            godmode = this.godmode,
            vomit = this.vomit,
            sound = this.sound,
        };
        if (drinkNames[0] == "Air")
        {
            drink.Delete();
            drink = null;
        }

        Cup cup = new()
        {
            Position = this.Position + (offset * this.Rotation),
            Rotation = this.Rotation,
            drink = drink,
        };

        if (drinkNames[0] != "Air")
        {
            drink.cup = cup;
            drink.Weld(cup);
        }

        switch (Language.SelectedCode)
        {
            case "en":
                Scp294Console.SayChat("SCP-294", drinkNames[0]);
                break;
            case "ru":
                Scp294Console.SayChat("SCP-294", drinkNamesRu[0]);
                break;
            case "uk":
                Scp294Console.SayChat("SCP-294", drinkNamesUk[0]);
                break;
            default:
                Scp294Console.SayChat("SCP-294", drinkNames[0]);
                break;
        }
    }

    public bool OnUse(Entity user)
    {
        if (useReloadMillis > useReload)
        {
            useReloadMillis = 0;

            if (clientKeyboard == null && !sound_use.IsPlaying) AddKeyboard(To.Single(user));
        }
        return false;
    }

    public bool IsUsable(Entity user)
    {
        return true;
    }

    public void UseLogic()
    {
        if (isTouched)
        {
            string lang = Language.SelectedCode;
            switch (lang)
            {
                case "en":
                    Scp294Console.SayChat("SCP-294", "Remove the cup from the coffee machine!");
                    break;
                case "ru":
                    Scp294Console.SayChat("SCP-294", $"Заберите стакан из кофемашины!");
                    break;
                case "uk":
                    Scp294Console.SayChat("SCP-294", $"Заберіть стакан з кофемашини!");
                    break;
                default:
                    Scp294Console.SayChat("SCP-294", $"Remove the cup from the coffee machine! [{lang}]");
                    break;
            }
        }
        else if (!sound_use.IsPlaying)
        {
            drinkIndex = FindDrinkInList(StripDrink(FindingName));

            if (drinkIndex != -1)
            {
                ParseDrinkProperties(drinkIndex);
                sound_use = Sound.FromEntity(dispenseSound, this);
                isUse = true;
                //SpawnParticle();
                DeletePanel();
                SpawnPanel("Dispensing...");
            }
            else
            {
                sound_use = Sound.FromEntity("outofrange", this);
                isOutOfRange = true;
                DeletePanel();
                SpawnPanel("Dispensing...");
            }
        }
    }

    public void DelaySpawn()
    {
        SpawnDrink();
    }

    protected override void OnDestroy()
    {
        sound_use.Stop();
        panel?.Delete(true);
        DeleteKeyboard();
        base.OnDestroy();
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
        if (dispenseSound == "dispense0") spawnTime = 1.5f; // TODO: Костиль бля
        if (dispenseSound == "dispense1") spawnTime = 2.7f;
        if (dispenseSound == "dispense2") spawnTime = 6.3f;
        if (dispenseSound == "dispense3") spawnTime = 6.6f;
        if (dispenseSound == "outofrange") spawnTime = 3.5f;

        if (sound_use.ElapsedTime > spawnTime && isUse)
        {
            DelaySpawn();
            DeletePanel();
            SpawnPanel(FindingName);
            isUse = false;
        }
        else if (sound_use.ElapsedTime > spawnTime && isOutOfRange)
        {
            DeletePanel();
            SpawnPanel("OUT OF RANGE");
            Scp294Console.SayChat("SCP-294", "OUT OF RANGE");
            isOutOfRange = false;
        }
    }

    string StripDrink(string name)
    {
        name = name.ToLower()
                    .Trim('.')
                    .Trim(',')
                    .Trim('\'')
                    .Replace("fluid ", "")
                    .Replace("liquid ", "")
                    .Replace("жидкий ", "")
                    .Replace("жидкая ", "")
                    .Replace("жидкое ", "")
                    .Replace("рідкий ", "")
                    .Replace("рідка ", "")
                    .Replace("рідке ", "")
                    .Replace("the", "")
                    .Replace("a cup of ", "")
                    .Replace("a cup ", "")
                    .Replace("cup of ", "")
                    .Replace("cup ", "")
                    .Replace("чашка ", "")
                    .Replace("чашечка ", "")
                    .Replace("стакан ", "")
                    .Replace("стаканчик ", "")
                    .Replace("склянка ", "")
                    .Replace("drink ", "")
                    .ToLower();
        return name;
    }

    int Map(int value, int fromMin, int fromMax, int toMin, int toMax)
    {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    void LogAll()
    {
        Log.Info($" ");
        Log.Info($" ");
        Log.Info($"FIND: [{FindingName.ToUpper().Trim('.').Trim(',').Trim()}]");
        Log.Info($"EN {drinkNames[0]}");
        //Log.Info($"RU {drinkNamesRu}");
        Log.Info($"UK {drinkNamesUk[0]}");
        //Log.Info($"Color {r}, {g}, {b}, {alpha}");
        Log.Info($"message: {message}");
        Log.Info($"messageRu: {messageRu}");
        Log.Info($"messageUk: {messageUk}");
        Log.Info($"lethal {lethal}");
        Log.Info($"deathTimer {deathTime}");
        Log.Info($"deathMessage: {deathMessage}");
        //Log.Info($"deathMessageRu: {deathMessageRu}");
        Log.Info($"deathMessageUk: {deathMessageUk}");
        Log.Info($"explosion {explosion}");
        Log.Info($"explosionTimer {explosionTime}");
        Log.Info($"stomachAche {stomachAche}");
        Log.Info($"blur {blur}");
        Log.Info($"glow {glow}");
        Log.Info($"refuse {refuse}");
        Log.Info($"refuseMessage: {refuseMessage}");
        //Log.Info($"refuseMessageRu: {refuseMessageRu}");
        //Log.Info($"refuseMessageUk: {refuseMessageUk}");
        Log.Info($"heal {heal}");
        Log.Info($"damage {damage}");
        //Log.Info($"godmode {godmode}");
        Log.Info($"vomit {vomit}");
        Log.Info($"{sound}");
        Log.Info($"{dispenseSound}");
    }
}