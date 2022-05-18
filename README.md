# NeonatologyProjectSite - https://pediamedbg.com/
This is a project, created for a doctor and her personal needs. There is an implemented functionality for patients to register and make appointments. It is also allowed for non-registered users to make appointments after they have filled the needed data. On successfull appointment they will receive an email with the date and time of their respective appointment. There is also an online chat between registered patients and the doctor.
# Диаграма на базата данни:
![dbDiagram](https://user-images.githubusercontent.com/58532362/168985804-df1b9900-adb8-40d0-bece-58d72270b1b2.png)

# The project uses:
- MS SQL
- EntityFramework Core
- ASP.NET Core
- Cloudinary
- Google SMTP service and MailKit
- Google CAPTCHA v3
- FullCalendar IO
- SignalR for notifications and chatting
- RPC for voice calls over chat
- Stripe
- Bootstrap and Font awesome
- Automapper
- Admin LTE 3 for administration panel
- XUnit and Moq for tests
- Hangfire

Как да работите с платформата:
![Screenshot from 2022-05-17 16-35-36](https://user-images.githubusercontent.com/58532362/168978654-4d076a33-1f94-42ff-8e85-80e7d7cdd5fd.png)

### За докторски профил:
За да се логнете в страницата, натиснете “Влез”:
![Screenshot from 2022-05-17 16-37-18](https://user-images.githubusercontent.com/58532362/168978870-a189021a-15b5-42fe-90c0-1b2c25f22763.png)

Този бутон ще Ви отведе на менюто за влизане:
![Screenshot from 2022-05-17 16-38-27](https://user-images.githubusercontent.com/58532362/168979136-97a68118-adc1-4081-9a0a-ff41b8166986.png)

Въведете и-мейла и паролата си и натиснете “Влез”. След това ще бъдете пренасочени към началната страница, като горе вдясно в менюто ще имате различни бутони:
![Screenshot from 2022-05-17 16-40-13](https://user-images.githubusercontent.com/58532362/168979142-cf68e6bd-e685-4365-a2cb-24ac00d7a851.png)

Като натиснете на бутон Доктор, ще се появи меню с различни полета:
![Screenshot from 2022-05-17 16-41-04](https://user-images.githubusercontent.com/58532362/168979148-da7ec50d-d7fb-4ee2-b8b1-759ff98967af.png)

Като натиснете на бутон Календар ще се появи следното меню:
![Screenshot from 2022-05-17 16-42-29](https://user-images.githubusercontent.com/58532362/168979155-e2acda07-287d-4d40-b23c-9560f5f19856.png)

Като натиснете върху някоя от датите ще излезе прозорец, чрез който може да си настроите работните часове:
![Screenshot from 2022-05-17 16-43-27](https://user-images.githubusercontent.com/58532362/168979164-e5162d9f-2fb2-4847-9be0-e8c39b084e24.png)

Изберете началният час, крайният час и интервалът през колко  минути, както и градът, за който ще бъдат генерирани тези часове:

![Screenshot from 2022-05-17 16-44-41](https://user-images.githubusercontent.com/58532362/168979165-6d1f3303-308a-417f-8963-30d76dc29dec.png)

В този случай ще бъдат генерирани часове от 08.00 ч. До 10.00 ч. За дата 19.05.2022 г. През интервал от 10 минути за гр. Плевен. 
След като изберете часовете и градът, натиснете бутон “Запази”. 
След успешно генериране на часове, календарът ще изглежда така: 
![Screenshot from 2022-05-17 16-46-29](https://user-images.githubusercontent.com/58532362/168979168-36624e7d-c750-4449-b96a-1bdd9f62c71a.png)

Когато натиснете върху някой от генерираните часове ще излезе следния прозорец.
![Screenshot from 2022-05-17 16-59-12](https://user-images.githubusercontent.com/58532362/168979186-e00b5901-7109-4cf5-9862-68dcab0c1249.png)

Чрез този прозорец може да променяте дали някой час е свободен или зает и да впишете причината, поради която е зает.

Като натиснете на меню “Съобщения” ще се появи това меню: 
![Screenshot from 2022-05-17 16-50-41](https://user-images.githubusercontent.com/58532362/168979172-0842bd2a-b3d0-4917-b21e-d6f63754e44d.png)
 Тук ще бъдат изписани всички потребители, с които имате проведена онлайн консултация. 
По време на провеждане на консултацията менюто ще изглежда така:
![Screenshot from 2022-05-17 16-52-08](https://user-images.githubusercontent.com/58532362/168979177-1324a147-5e68-4dbc-8d96-b74c9bfcaf8a.png)

Горе вляво имате запитване от браузъра дали позволявате достъп до вашия микрофон. Трябва да натиснете “Allow/Позволи”, за да можете да провеждате онлайн разговор с клиента. Ако сте натиснали “Block/Блокирай/Не позволявай” ще трябва да обновите страницата, ако желаете да провеждате такъв разговор. 

Горе вдясно имате 3 прозореца:
![Screenshot from 2022-05-17 16-54-36](https://user-images.githubusercontent.com/58532362/168979178-e8e3f32b-19e4-4390-9280-a49597bc8c5c.png)
Чрез микрофонът Вие набирате другия потребител за провеждане на разговор. Ако той не е налиния няма да може да проведете разговор и ще получите съобщение, че потребителя не е налиния. Чрез бутонът с камера може да качвате снимки, а чрез бутонът с кламер може да качвате документи. 

Когато в кръглото поле има цифра означава, че сте прикачили съответния документ/снимка и чака да бъдат изпратени чрез натискане на бутона “Изпрати”.
![Screenshot from 2022-05-17 16-57-09](https://user-images.githubusercontent.com/58532362/168979181-bb23e0dc-5fb2-42e5-a9bd-b7ee9292f1b8.png)

### Нерегистрирани потребители:
За да запазите час за преглед натиснете бутонът "Запази час":
![Screenshot from 2022-05-18 10-21-55](https://user-images.githubusercontent.com/58532362/168980985-40828a36-ce65-48b8-865e-e9acf54c50db.png)

Ще бъдете отведени в следното меню:
![Screenshot from 2022-05-18 10-23-08](https://user-images.githubusercontent.com/58532362/168981248-0e5cb13a-d4ab-4286-b6d7-a8eb7735cf77.png)

В зелено са всички свободни часове, които докторът е вписал в своя график. Горе, вляво, има бутони "Плевен" и "Габрово". Чрез тях ще може да видите свободните часове за съответния град.
За да запазите своя час, натискате върху някой от свободните часове и ще се появи следното меню:
![Screenshot from 2022-05-18 10-25-42](https://user-images.githubusercontent.com/58532362/168981734-73df45b3-1352-4aa4-8002-6fc8026de69a.png)

Въведете своите данни и натиснете бутонът "Запази час". При успешно запазване ще бъдете пренасочени към началната страница и ще получите и-мейл с информацията на посочения от Вас и-мейл адрес.

### Регистрирани потребители:
За да запазите час за преглед натиснете бутонът "Запази час":
![Screenshot from 2022-05-18 10-21-55](https://user-images.githubusercontent.com/58532362/168980985-40828a36-ce65-48b8-865e-e9acf54c50db.png)

Ще бъдете отведени в следното меню:
![Screenshot from 2022-05-18 10-23-08](https://user-images.githubusercontent.com/58532362/168981248-0e5cb13a-d4ab-4286-b6d7-a8eb7735cf77.png)

В зелено са всички свободни часове, които докторът е вписал в своя график. Горе, вляво, има бутони "Плевен" и "Габрово". Чрез тях ще може да видите свободните часове за съответния град.
За да запазите своя час, натискате върху някой от свободните часове и ще се появи следното меню:
![Screenshot from 2022-05-18 10-29-46](https://user-images.githubusercontent.com/58532362/168982513-6e01d1fc-4143-4ded-b224-192722e1180d.png)

Въведете необходимите данни и натиснете бутонът "Влез". При успешно записване на час, ще получите и-мейл с информация на и-мейл адресът, с който сте се регистрирали. Ще бъдете пренасочени към страницата с Вашите предстоящи часове.

За да заявите онлайн консултация натиснете върху бутън "Съобщения":
![Screenshot from 2022-05-18 10-39-13](https://user-images.githubusercontent.com/58532362/168984272-b5fb74bf-3769-409c-a53f-e1505a25b8fe.png)

Ще бъдете пренасочени към следната страница:
![Screenshot from 2022-05-18 10-39-50](https://user-images.githubusercontent.com/58532362/168984506-9c2cc0ff-69b9-48da-a8c2-796bf668f57c.png)

Тук ще може да видите предишни проведени онлайн консултации, ако има такива. За да заявите нова онлайн консултация натиснете върху бутона "Заяви онлайн консултация" и ще бъдете пренасочени към меню, където ще имате информация за съответната поръчка, която ще направите:
![Screenshot from 2022-05-18 10-42-50](https://user-images.githubusercontent.com/58532362/168984990-e59c3764-c752-4740-91fd-dd297d9068b9.png)

Като натиснете "Към плащане" ще се появи следното меню:
![Screenshot from 2022-05-18 10-44-19](https://user-images.githubusercontent.com/58532362/168985254-9a0a1a87-f458-4399-a08a-dc49f698ec6b.png)

Въвеждате необходимите данни и натискате бутон "Pay" и ще имате 24 часа от момента на обработването на плащането, за да проведете онлайн консултацията. Ако в тези 24 часа не сте я провели, ще трябва да заплатите отново.
