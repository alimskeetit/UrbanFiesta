﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Entities
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Citizen> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(AppDbContext context, UserManager<Citizen> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private List<Citizen> citizens = new List<Citizen>
        {
            new()
            {
                FirstName = "Василий",
                LastName = "Раздорский",
                Patronymic = "Юрьевич",
                Email = "vasiliy@mail.ru",
                UserName = "vasiliy@mail.ru",
                BirthDate = DateTime.Parse("2003-04-25")
            },
            new()
            {
                FirstName = "Иван",
                LastName = "Алимский",
                Patronymic = "Денисович",
                Email = "vanya@mail.ru",
                UserName = "vanya@mail.ru",
                BirthDate = DateTime.Parse("2003-08-30")
            }
        };

        public async Task SeedUsers()
        {
            if (_userManager.Users.Any()) return;
            await _userManager.CreateAsync(citizens[0], "123");
            await _userManager.AddToRolesAsync(citizens[0], roles: new List<string> { "user", "admin" });
            await _userManager.CreateAsync(citizens[1], "123");
            await _userManager.AddToRolesAsync(citizens[1], roles: new List<string> { "user", "admin" });
        }

        public async Task SeedRoles()
        {
            if (_roleManager.Roles.Any()) return;
            await _roleManager.CreateAsync(new IdentityRole("user"));
            await _roleManager.CreateAsync(new IdentityRole("admin"));
        }

        public async Task SeedEvents()
        {
            var events = _context.Set<Event>();
            if (_context.Set<Event>().Any()) return;
            await events.AddRangeAsync(
                new[]
                {
                    new Event
                    {
                        Title = "Macan",
                        Description = "Его имя известно всем. Каждый новый релиз занимает высшие строчки во всех музыкальных топ-чартах и звучит во всех заведениях твоего района.\\n\\nMacan выступит с новой программой, со всеми супер-хитами и лайв-бэндом только для того, чтобы ты получил все эмоции на масштабном неповторимом шоу и оставил этот день в своей памяти на всю жизнь.",
                        Address = "Ленинградский просп., 36",
                        Coordinates = "55.790908754843464|37.55874995190431",
                        StartDate = DateTime.Parse("2023-05-21T19:00:00"),
                        EndDate = DateTime.Parse("2023-05-22T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/23114/6a586504ffc089cbc382aa6a5eed69a7/s760x380"
                    },
                    new Event
                    {
                        Title = "Панк-культура. Король и Шут",
                        Description = "\nБудьте как дома, панки! А ещё любители, интересующиеся, выросшие из этого и так далее — здесь рады всем. Это та самая выставка, где информационный дисплей оказался в писсуаре, телефонная будка на боку, а панк повсюду — на стенах, потолках, лестницах и просто в воздухе.\n\nНа выставке целых два тематических зала:\n\nПервый зал:\n\nВ нём вам докажут, что панк — не просто громкая музыка, а целый культурный феномен. Здесь панк-мода в костюмах и футболках, воссозданные культовые заведения, плакаты, инсталляции и видео. Путешествуйте из легендарного нью-йоркского клуба CBGB и лондонского магазина SEX в монументальные советские ДК и дальше — на большие площадки настоящего. В этом зале вы увидите винтажные вещи иконы панк-стиля Вивьен Вествуд, арт-объекты английского художника Джейми Рида, создавшего визуальный стиль Sex Pistols, и воссозданную домашнюю студию Егора Летова.\n\nВторой зал:\n\nПутешествие в мир «Короля и Шута»: в декорациях замка, таверны, леса и кладбища вы узнаете о бесшабашных выступлениях в клубе Tamtam и главных альбомах группы, попадёте в полный Олимпийский и найдёте свидетельства народной славы, исследуете тайные смыслы текстов песен и заглянете на съемочную площадку сериала по мотивам истории группы. Здесь представлены сценические костюмы музыкантов, рукописи песен, редкие кассеты и диски, афиши и декорации сериала.\n\nНагулялись, вдохновились и захотели немного панка с собой? Это можно: зайдите в магазин Яндекс Маркета прямо на площадке. В нём — эксклюзивная одежда и сувениры по мотивам выставки. А если у вас есть Яндекс Плюс — не забудьте заглянуть в VR-пространство с клипом на песню «Короля и Шута» в формате 360°.\n\nВыставку придумали и сделали Плюс Студия, Яндекс Афиша и Planet 9 (создатели выставок «Виктор Цой. Путь героя», «Балабанов»).",
                        Address = "4-й Сыромятнический пер., 1/8, стр. 6",
                        Coordinates = "55.75550652175932|37.6651870325287",
                        StartDate = DateTime.Parse("2023-05-31T19:00:00"),
                        EndDate = DateTime.Parse("2023-06-01T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/21626/5da6c6ca477c4988653a1946b85b008b/s760x380"
                    },
                    new Event
                    {

                        Title = "Московский джазовый фестиваль",
                        Description = "19 июня состоится открытие второго международного Московского джазового фестиваля. В этот торжественный вечер совместную программу представят Народный артист России Игорь Бутман, Московский джазовый оркестр и Народная артистка России Лариса Долина, а также специальные гости.\n\nMoscow Jazz Festival — один из самых масштабных джазовых фестивалей мира, пройдёт в 2023 году в столице во второй раз. В общей сложности на Фестивале выступят более 1000 артистов, а хедлайнерами станут хорошо известные джазмены и популярные артисты других жанров, которые представят джазовые программы. Открытие и Закрытие Фестиваля пройдут на сцене Зала «Зарядье, а также, на протяжении пяти дней, именитые музыканты и молодежные джаз-коллективы, отобранные по итогам Всероссийского конкурса. А также гостей Фестиваля ждет масштабная музыкальная программа в ведущих джазовых клубах столицы и образовательная программа в парках.\n\n20 июня в Саду «Эрмитаж» для гостей фестиваля уникальные программы представят: Валерий Сюткин и Light Jazz, Трио Олега Аккуратова, а также зарубежные музыканты. Также гости Фестиваля смогут принять участие в творческих мастер-классах, а для юных зрителей будут организованы специальные активности от партнеров.\n\n21 июня в Саду «Эрмитаж» выступят Варвара Убель и Квартет Олега Бутмана, а также известные кубинские джазмены, хедлайнером вечера станет МОТ, который представит специальную программу. Также гости Фестиваля смогут принять участие в творческих мастер-классах, а для юных зрителей будут организованы специальные активности от партнеров.\n\n22 июня на Фестивале в «Эрмитаже» выступит ансамбль «Терем-квартет», Григор Вернер и Трио Андрея Кондакова, а также Эстрадный оркестр Сергея Мазаева. Также гости Фестиваля смогут принять участие в творческих мастер-классах, а для юных зрителей будут организованы специальные активности от партнеров.\n\nВ пятницу, 23 июня, свои новые программы представят Вадим Эйленкриг & Eilenkrig Crew, известные бразильские и российские музыканты. Также гости Фестиваля смогут принять участие в творческих мастер-классах, а для юных зрителей будут организованы специальные активности от партнеров.\n\n24 июня хедлайнерами фестивальной программы в Саду «Эрмитаж» станут Игорь Бутман и Московский джазовый оркестр, которые представят совместную программу с одним из самых востребованных оперных певцов современности, дважды обладателем премии «Грэмми» — Ильдаром Абдразаковым. Также в этот день выступят известные российские и иностранные артисты.\n\nПомимо музыкальной программы для гостей Фестиваля пройдёт множество творческих мастер-классов, а для юных зрителей будут организованы специальные активности от партнеров.\n\n25 июня на закрытии второго международного Московского джазового фестиваля Народные артисты России Игорь Бутман и Алексей Гуськов в сопровождении Московского джазового оркестра представят премьеру музыкально-театрального перформанса.",
                        Address = "Varvarka Street, 6с4",
                        Coordinates = "55.751279290455834|37.63142191529412",
                        StartDate = DateTime.Parse("2023-06-19T19:00:00"),
                        EndDate = DateTime.Parse("2023-06-20T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/21422/25b7ed65493342f9d7e13bc2c0887576/960x690_noncrop"
                    },
                    new Event
                    {

                        Title = "Фестиваль «Будущее»",
                        Description = "Фестиваль «Будущее» — это музыкальный фестиваль молодых и талантливых артистов, которые уже собирают крупнейшие залы поклонников по всей России или только начинают свой творческий путь. Главное, что объединяет артистов фестиваля — эмоции, которые они передают слушателям, погружая их в особенную атмосферу своего выступления.\n\nНа фестивале «Будущее» в 2022 и 2023 году уже выступали такие артисты как Три дня дождя, Pyrokinesis, Мукка, Хаски, GSPD, Lida, Кишлак, CMH, playingtheangel, Космонавтов нет, Папин Олимпос, saypink!, Aikko и многие другие. Отдельно отметим, что зимний фестиваль, который состоялся в клубе VK Stadium в Москве и Гигант Холл в Санкт-Петербурге в конце января 2023 года, собрал более 20000 гостей, став крупнейшим зимним клубным фестивалем России за последние несколько лет.\n\nКаждый новый фестиваль организаторы расширяют программу — летом 2023 года на фестивале, который будет разбит на несколько уик-ендов, выступят более 40 артистов, при этом фестиваль состоится сразу в нескольких крупнейших локациях столицы: с разным настроением и концепцией шоу.\n\nВозрастные ограничения отсутствуют при соблюдении правил посещения VK Stadium. Для входа в зрительный зал потребуется билет и паспорт (загранпаспорт, водительское удостоверение или военный билет). Копии и фото документов для входа не принимаются. Гости, у которых не будет с собой оригинала одного из вышеперечисленных документов, могут посетить концерт с мамой или папой — вход для родителя будет бесплатным (предоставляется билет в танцпол).",
                        Address = "Ленинградский просп., 80, стр. 17",
                        Coordinates = "55.807738614967334|37.51168665030401",
                        StartDate = DateTime.Parse("2023-07-07T19:00:00"),
                        EndDate = DateTime.Parse("2023-07-08T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/31447/a9179769e768cb859405656ee990fa96/960x690_noncrop"
                    },
                    new Event
                    {

                        Title = "Chess & Jazz",
                        Description = "В московском саду «Эрмитаж» пройдет четвертый фестиваль шахмат и джаза Chess & Jazz. На один уикенд зелёный оазис в самом центре столицы превратится в джазовую гостиную под открытом небом, где каждый сможет сыграть шахматную партию бок о бок с гроссмейстерами мирового уровня и просто насладиться летним днем. Организаторы создадут магическую обстановку полную хорошей музыки и интеллектуальных упражнений. Не обойдется и без авторской подборки ресторанных проектов от Александра Сысоева. Chess & Jazz вновь станет центром притяжения единомышленников, поклонников джаза и шахмат, интеллектуальных игр и душевного общения.\n\nГород под названием Chess & Jazz, которого нет ни на одной карте, но который появляется раз в году откроет свои двери для дорогих друзей.\n\nПосетители младше 12 лет (это значит, что ребенку не исполнилось 12 лет) могут пройти на фестиваль бесплатно, но только в сопровождении взрослых. Не забудьте взять с собой свидетельство о рождении, чтобы подтвердить возраст ребенка.",
                        Address = "ул. Каретный Ряд, 3",
                        Coordinates = "55.77068857953324|37.60917163472824",
                        StartDate = DateTime.Parse("2023-07-29T19:00:00"),
                        EndDate = DateTime.Parse("2023-07-30T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/21626/3f43bb43122fc25b5e5b55a4bc7db530/960x690_noncrop"
                    },
                    new Event
                    {

                        Title = "Locals Only 2023",
                        Description = "Locals Only — объединяет современную музыку и экстремальные виды спорта, общение и активный отдых. Название фестиваля Locals Only переводится как «только для местных» и дает каждому возможность найти свой круг единомышленников, искренность общения близких по духу людей.\n\nНаша жизнь — это стремительные виражи, удержаться на которых непросто, но мы знаем, как это сделать. Изменить сознание, сломать привычные рамки и стереотипы, не размениваясь на мелочи, взять высшую планку, за которой открывается путь в будущее.\n\nИменно поэтому, Locals Only — это не только музыка, у которой нет границ, разные стили, актуальные артисты, качественный звук и впечатляющее шоу. Это встреча лучших представителей различных видов экстремального спорта, известных медиа-персон, уникальная программа развлечений и активного дружеского общения и отдыха.\n\nВ 2023 году фестиваль Locals Only предлагает своим гостям выступления суперзвезд российской музыкальной сцены самых различных направлений: рок, панк, панк-рок. Вас ждёт скейт-парк и соревнования по скейтбордингу, турнир по стритболу, батл лучших брекдансеров, игровые зоны, фотовыставка, фуд-зоны, лаунж зоны и многие другие активности от наших партнёров.\n\nУчастники 10 июня: Три Дня Дождя, Мукка, Космонавтов нет, Заточка, By Индия, СА:МИ, Denny Todd и другие.\nУчастники 11 июня: Ram, Папин Олимпос, Neverlove, Щенки, Пол Пунш, Завтра Брошу, Denny Todd и другие.\n\nБлагодаря насыщенной программе каждый сможет выбрать свой стиль музыки, вид спорта, стать частью сообщества поклонников активного отдыха. Это уникальный шанс для молодежи узнать больше об экстремальных видах спорта и актуальной музыке, приятно и интересно провести время, посмотреть выступления выдающихся спортсменов; потусоваться со старыми друзьями и познакомиться с новыми.\n\nLocals Only 2023 — это место встречи музыки и спорта, атмосфера праздника и яркие впечатления для юных душой и сердцем. Это настоящая культура и философия свободы в смутные времена ограничений.\n\nБудь собой. Стань частью этого уникального и незабываемого события. Присоединяйся и заряжайся энергией вместе с нами!",
                        Address = "ул. Шарикоподшипниковская, 13, стр. 33",
                        Coordinates = "55.72031713151314|37.68466418584828",
                        StartDate = DateTime.Parse("2023-06-10T19:00:00"),
                        EndDate = DateTime.Parse("2023-06-11T00:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/35821/7ff1447464203410c6199044e1ae613a/960x690_noncrop"
                    },
                    new Event
                    {

                        Title = "Международный день йоги в России",
                        Description = "Празднование дня начнётся с массового выполнения одной из самых известных в мире последовательностей оздоровительных упражнений Сурья Намаскар (Приветствие солнцу) 108 раз, после чего в 12 откроются более пятидесяти площадок с занятиями по различным направлениям йоги с известными учителями из ведущих школ Москвы, медитация, аюрведа и лекции по темам здорового образа жизни, красоты, правильного питания и саморазвития.\\n\\nТакже планируется провести на главной сцене самую массовую медитацию в России, к который сможет присоединиться каждый желающий, не имеющий даже предварительного опыта.\\n\\nЛюбителей музыки ожидают сцены с живыми выступлениями артистов разных направлений — киртан (мантры), электро, а также отдельная танцевальная площадка с мастер-классами.\\n\\nТакже будет работать детская зона и Organic People Market, на котором развернется грандиозная ярмарка тематической продукции — дизайнерская и йога одежда, редкие изделия ручной работы из разных стран мира, здоровое питание, натуральная косметика, экотовары и вегетарианский фудкорт.\\n\\nОрганизаторы фестиваля — компания Organic People, занимающаяся популяризацией йоги и здорового образа жизни с 2012-го года и известная своими социальными проектами «Йога в парках», «Медитация в парках» и организацией Международного дня йоги в России, отмечавшегося в прошлые года.\\n\\nВ 2022-ом году фестиваль посетило более 100 000 человек, в этом году ожидаемое число гостей более 130 000 человек.\\n\\n25 июня 8:00–23:00\\nВход на фестиваль бесплатный\\nЗанятия в \r\nшатрах по единому билету",
                        Address = "ул. Дольская, 1",
                        Coordinates = "55.6164036973777|37.68334905548407",
                        StartDate = DateTime.Parse("2023-06-25T08:00:00"),
                        EndDate = DateTime.Parse("2023-06-25T23:00:00"),
                        PosterUrl = "https://avatars.mds.yandex.net/get-afishanew/21626/643b7b71683ffaac212c442419693935/960x690_noncrop"
                    }
                }
                );
            await _context.SaveChangesAsync();
        }

        public async Task SeedAsync()
        {
            await SeedRoles();
            await SeedUsers();
            await SeedEvents();
        }
    }
}
