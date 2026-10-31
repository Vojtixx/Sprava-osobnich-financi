Správa osobních financí



Autor: Vojtěch Říha
Třída: ITB4

Popis aplikace



Mobilní aplikace vytvořená v .NET MAUI slouží ke správě osobních financí. Umožňuje sledovat příjmy, výdaje, aktuální zůstatek a měsíční rozpočet. Uživatel může přidávat nové transakce, zobrazovat aktuální kurzy měn z webu ČNB a přepínat mezi světlým a tmavým motivem. Aplikace také umožňuje ukládat a načítat přihlašovací údaje do JSON souboru.

Rozhraní vychází z Material Design 3 a obsahuje čtyři obrazovky: Přehled, Nová transakce, Kurzy měn a Nastavení.



Zápis do JSON

Na stránce Nastavení lze uložit uživatelské jméno a heslo. Údaje se ukládají do souboru UserCredentials.json ve složce aplikace (FileSystem.AppDataDirectory), kam má aplikace na telefonu právo zapisovat. Objekt třídy UserCredentials (jméno, heslo, čas uložení) se pomocí knihovny System.Text.Json převede na text (JsonSerializer.Serialize) a zapíše asynchronně metodou File.WriteAllTextAsync. Při načtení se soubor přečte a převede zpět na objekt (JsonSerializer.Deserialize). Uložit lze jen platné jméno a silné heslo, tlačítko je během ukládání vypnuté a chyby při práci se souborem jsou ošetřené pomocí try/catch a hlášky DisplayAlert. Uložené údaje lze také smazat. Zápis do JSON funguje.



Využití AI

Při vývoji byl využit AI asistent Claude (Anthropic) prostřednictvím Claude Code, především pro:

* Návrh a úpravu struktury projektu
* Vytvoření uživatelského rozhraní v XAML a designového systému
* Tvorbu ikon a grafických prvků
* Implementaci navigace mezi stránkami
* Pomoc s přepínáním světlého a tmavého motivu
* Vytvoření dokumentace README



Samostatně jsem implementoval programovou logiku, zejména validaci formulářů, ukládání transakcí, výpočty a filtrování na hlavní obrazovce, kontrolu síly hesla, práci s JSON souborem a ošetření chyb.

