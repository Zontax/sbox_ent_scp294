using System.Collections.Generic;

namespace Sandbox;

internal static class DrinkDataList
{
    public static List<string> drinks = new()
    {
        @"
        [Water|H2O]
        ru[Вода|Водичка]
        uk[Вода]
        color=50,120,255
        alpha=0.35
        message={Well, that was refreshing}
        messageRu={Что ж, это было освежающе}
        messageUk={Що ж, це було освіжаюче}
        sound={ahh}
        ",
        @"
        [Air|Oxygen|Nothing|Cup|Vacuum|Void|Emptiness|Nope|Null|Zero|HL3|Half-Life 3|Half Life 3]
        ru[Воздух|Кислород|Ничего|Чашка|Вакуум|Пустота]
        uk[Повітря|Кисень|Нічого]
        color=255,255,255
        alpha=0.6
        message={There is nothing to drink in the cup}
        refuse=true
        refuseMessage={There is nothing to drink in the cup}
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
        messageRu={Просто свежий апельсиновый сок}
        messageUk={Просто свіжий апельсиновий сік}
        sound={ahh}
        ",
        @"
        [Tomato Juice|Tomato|Tomat]
        ru[Томатный сок|Помидор]
        uk[Томатний сік|Помідор]
        color=204,0,0
        alpha=0.9
        message={Meh, it's ok. Never was a big fan of tomato juice though}
        messageRu={Все в порядке. Хотя я не большой поклонник томатного сока}
        messageUk={Все в порядку. Хоча я не великий пошановувач томатного сока}
        sound={ahh}
        ",
        @"
        [Beer|Lager|Pale Beer|Light Beer]
        ru[Пиво|Светлое пиво|Пивко]
        uk[Пиво|Світле пиво]
        color=235,165,40
        alpha=0.8
        message={To taste the usual light beer, cold}
        messageRu={По вкусу обычное светлое пиво, холодное}
        messageUk={За смаком звичайне світле пиво, холодне}
        sound={ahh}
        ",
        @"
        [Coffee|Black Coffee|Coffe|Black Coffe]
        ru[Кофе|Чорный кофе]
        uk[Кава|Чорна кава]
        color=87,62,45
        message={The drink tastes like fairly strong black coffee}
        ",
        @"
        [Espresso]
        ru[Эспрессо]
        uk[Еспрессо]
        color=95,65,47
        message={The drink tastes like fairly tasty espresso}
        messageRu={По вкусу напиток напоминает довольно вкусный эспрессо}
        messageUk={По смаку напій нагадує досить смачний еспрессо}
        ",
        @"
        [Americano]
        ru[Американо]
        uk[Американо]
        color=90,60,45
        message={The drink tastes like fairly strong americano}
        ",
        @"[Nuke Coffee|Nuke Coffe]
        ru[Ядерный кофе|Ядерное кофе]
        uk[Ядерна кава]
        color=87,142,45
        lethal=true
        deathTimer=6
        deathMessage={Not the same coffee as in Fallout}
        deathMessageRu={Не тот кофе, что в Fallout}
        deathMessageUk={Не та кава, що в Fallout}
        blur=20
        damage=15
        sound={ew1}
        dispenseSound={dispense3}
        ",
        @"
        [Latte|Caffe latte]
        ru[Латте]
        uk[Лате]
        color=250,200,250
        message={The drink tastes like latte}
        ",
        @"
        [Pumpkin latte]
        ru[Тыквенный латте]
        uk[Гарбузовий лате]
        color=255,136,0
        message={The drink tastes like pumpkin latte}
        messageRu={Напиток реально по вкусу напоминает тыквенный латте}
        messageUk={Напиток реально по вкусу напоминает тыквенный латте}
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
        message={You feel sick, maybe radiation is not what you need}
        messageRu={Вам стало плохо, возможно радиация не то что вам нужно} 
        messageUk={Вам стало зле, можливо радіація не те що вам потрібно} 
        lethal=true
        deathTimer=3
        deathMessage={Subject ##### was found lying in a small puddle of depleted uranium. Cause of death was confirmed to be acute radioactive poisoning}
        deathMessageRu={Объект ##### был найден лежащим в небольшой луже обедненного урана. Причиной смерти было признано острое радиоактивное отравление}
        deathMessageUk={Суб'єкт ##### був знайдений лежачим у невеликій калюжі зі збідненим ураном. Причиною смерті було підтверджено гостре радіоактивне отруєння}
        damage=76
        sound={burn}
        dispenseSound={dispense3}
        ",
        @"
        [Bleach]
        ru[Отбеливатель]
        uk[Відбілювач]
        color=255,255,250
        message={The liquid burns in your mouth and throat}
        messageRu={Эта жидкость жжет во рту и горле} 
        messageUk={Ця рідина пече в роті та горлі} 
        lethal=true
        deathTimer=5
        deathMessage={The liquid burns in your mouth and throat. You dead}
        deathMessage={Жидкость жжет ваш рот и горло. Вы умерли}
        deathMessageUk={Рідина пече у вашому роті та горлі. Ви мертві}
        damage=90
        sound={burn}
        dispenseSound={dispense2}
        ",
        @"
        [Mucus]
        ru[Слизь|Cлизи|Лизун]
        uk[Слиз|Слизу]
        color=102,204,0
        alpha=0.9
        refuse=true
        refusemessage={Why the hell would I want to drink this?}
        refusemessageUk={Какого черта я должен это пить?}
        refusemessageRu={Якого біса я маю це пити?}
        dispenseSound={dispense2}
        ",
        @"
        [Nausea]
        ru[Тошнота]
        uk[Нудота]
        color=59,92,32
        alpha=0.6
        refuse=true
        refusemessage={Why the hell would I drink that? It smells like garbage}
        refusemessageRu={Какого черта я должен это пить? От него воняет помоями.}
        refusemessageUk={Якого біса я маю це пити? Від нього тхне помиями.}
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
        messageRu={Обычная пепси-кола}
        messageUk={Звичайна пепсі-кола}
        sound={ahh}
        ",
        @"
        [Pepsi Cherry|Pepsi-cola Cherry|Pepsi Wild Cherry|Pepsi-cola Wild Cherry]
        ru[Вишневая Пепси|Пепси-кола вишня|Пепси вишня|Пепси дикая вишня]
        uk[Вишнева Пепсі|Вишнева Пепсі-кола|Пепсі вишня|Пепсі дика вишня]
        color=86,20,5
        message={Just Pepsi-cola.}
        messageRu={Обычная пепси-кола}
        messageUk={Звичайна пепсі-кола}
        sound={ahh}
        ",
        @"
        [Fanta|Orange Fanta|Fanta Orange]
        ru[Фанта, Апельсиновая фанта|Фанта со вкусом апельсина]
        uk[Фанта, Апельсинова фанта|Фанта зі смаком апельсина]
        color=240,175,70
        alpha=0.8
        message={Just orange Fanta.}
        messageRu={Обычная апельсиновая Fanta}
        messageUk={Звичайна апельсинова Fanta}
        sound={ahh}
        ",
        @"
        [Sprite]
        ru[Спрайт]
        uk[Спрайт]
        color=200,225,200
        alpha=0.6
        message={Just Sprite.}
        messageRu={Обычный Sprite}
        messageUk={Звичайний Sprite}
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
        ru[Энергетический напиток|Энергетик|Энергос]
        uk[Енергетичний напій|Енергетик]
        color=50,215,70
        alpha=0.9
        message={It tastes slightly strong and tart.}
        messageRu={На вкус слегка крепкий и терпкий.}
        messageUk={Смак трохи міцний і терпкий.}
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
        message={The mouth cavity, throat and face of the victim are covered in severe third degree burns. A layer of black, crystalline substance was found inside the mouth cavity. Sent for chemical analysis}
        messageRu={Ротовая полость, горло и лицо жертвы покрыты сильными ожогами третьей степени. Внутри ротовой полости обнаружен слой черного кристаллического вещества. Отправлено на химический анализ}        
        messageUk={Ротова порожнина, горло та обличчя потерпілого вкриті сильними опіками третього ступеня. У ротовій порожнині виявлено шар чорної кристалічної речовини. Відправлено на хімічний аналіз}
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
        message={It really is someone's blood}
        messageRu={Это похоже чья-то кровь}
        messageUk={Це похоже чиясь кров}
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
        message={Looks like you have leukemia}
        messageRu={Похоже вы заболели лейкимией}
        messageUk={Схоже ви захворіли лейкімією}
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
        message={You felt a tingling sensation in your throat}
        messageRu={Вы почувствовали покалывание в горле}
        messageUk={Ви відчули поколювання в горлі}
        lethal=true
        deathTimer=15
        deathMessage={You died from internal bleeding of the stomach}
        deathMessageRu={Вы умерли от внутреннего желудочного кровотечения}
        deathMessageUk={Ви померли від внутрішньої кровотечі шлунку}
        stomachAche=true
        blur=10
        damage=90
        vomit=true
        sound={bee}
        dispenseSound={dispense2}
        ",
        @"
        [Gold|Golden Drink|Gold Drink|Aurum]
        ru[Золото|Золотой напиток|Аурум]
        uk[Золото|Золотий напій]
        color=255,200,15
        glow=true
        message={You felt a tingling sensation in your throat}
        messageRu={Вы почувствовали покалывание в горле}
        messageUk={Ви відчули поколювання в горлі}
        lethal=true
        deathTimer=2
        deathMessage={You died from an internal burn}
        deathMessageRu={Вы умерли от внутреннего ожога}
        deathMessageUk={Вы умерли от внутреннего ожога}
        damage=77
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
        deathMessage={You died from an internal burn}
        deathMessageRu={Вы умерли от внутреннего ожога}
        deathMessageUk={Вы умерли от внутреннего ожога}
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
        deathMessage={Eating nitrogen... genius}
        deathMessageRu={Съесть нитроген.. гений}
        deathMessageUk={З'їсти нітроген.. геній}
        stomachAche=true
        blur=10
        damage=30
        vomit=true
        sound={slurp}
        dispenseSound={dispense2}
        ",//Something to destroy 682
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
        message={This is tasty milk}
        messageRu={Это вкусное молоко}
        messageUk={Це смачне молоко}
        ",
        @"
        [Chocolate Milk]
        ru[Шоколадное молоко]
        uk[Шоколадне молоко]
        color=150,50,0
        alpha=0.6
        message={Just chokolate milk}
        messageRu={Обычное шоколадное молоко}
        messageUk={Звичайне шоколадне молоко}
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
        message={I won't drink this, it looks and smells disgusting}
        messageRu={Я не буду это пить оно выглядит и пахнет отвратительно}
        messageUk={Я не буду це пити воно виглядає та пахне огидно}
        refuse=true
        refuseMessage={I'm not going to drink it, it looks and smells disgusting}
        refuseMessageRu={Я не буду это пить оно выглядит и пахнет отвратительно}
        refuseMessageUk={Я не буду це пити воно виглядає та пахне огидно}
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
        message={Mmm... so sweet}
        messageRu={Ммм... так сладко}
        messageUk={Ммм... так солодко}
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
        refuseMessage={When you reach for the cup, you clumsily knock it to the ground and the contents spill all over the floor. How ironic}
        refuseMessageRu={Потянувшись за чашкой, вы неуклюже опрокидываете ее на пол, и содержимое проливается на пол. Как иронично}
        refuseMessageUk={Коли ви потяглися до чашки, ви незграбно перекинули її на підлогу, і вміст розлився по всій підлозі. Як іронічно}
        sound={none}
        dispenseSound={dispense3}
        ",
        @"
        [Acid|Acid Liquid|Liquid Acid|Poison]
        ru[Кислота|Кислотная смесь|Супер кислота]
        uk[Кислота|Кислотний напій]
        color=10,90,10
        glow=true
        message={Acid kills}
        messageRu={Кислота убивает}
        messageUk={Кислота вбиває}
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
        message={Bad trip.}
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
        damage=99999
        dispenseSound={dispense3}
        ",
        @"
        [Bomb|Explosive|Explosives|Semtex|Grenade]
        ru[Бомба|Взрывчатка|Взрыв|Бомбочка|Взрывоопасное]
        uk[Бомба|Вибухівка|Вибух|Вибухонебезпечне|Бімба]
        color=11,12,13
        message={The bomb makes a boom...}
        messageRu={Бомба делает бум.}
        messageUk={Бомба робить бум.}
        deathMessage={Shit...}
        explosion=true
        damage=160
        dispenseSound={dispense3}
        ",
        @"
        [Surprise me|Surprise]
        ru[Удиви меня]
        uk[Здивуй мене]
        color=11,12,250
        message={An opaque glass containing normal-looking water, which, as was later determined, was actually heated to approximately 200°C. When the glass was picked up, the vibration caused its contents to turn into steam, splashing boiling water over a 2-meter radius.}
        messageRu={Непрозрачный стаканчик, содержавший нормальную с виду воду, которая, как позже было установлено, на самом деле была нагрета примерно до 200°С. Когда стаканчик был взят в руки, от вибрации его содержимое превратилось в пар, разбрызгивая кипящую воду в 2-метровом радиусе.}
        messageUk={Непрозорий стаканчик, що містив нормальну на вигляд воду, яка, як пізніше було встановлено, насправді була нагріта приблизно до 200°С. Коли стаканчик було взято до рук, від вібрації його вміст перетворився на пару, розбризкуючи киплячу воду в 2-метровому радіусі.}
        lethal=false
        deathTimer=10
        deathMessage={Shit...}
        explosion=true
        damage=50
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
}
