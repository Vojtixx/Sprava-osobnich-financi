Správa osobních financí

Autor: Vojtěch Říha
Třída: ITB4

Popis aplikace

Mobilní aplikace vytvořená v .NET MAUI slouží ke správě osobních financí. Umožňuje sledovat příjmy, výdaje, aktuální zůstatek a měsíční rozpočet. Uživatel může přidávat nové transakce, zobrazovat aktuální kurzy měn z webu ČNB a přepínat mezi světlým a tmavým motivem. Aplikace také umožňuje ukládat a načítat přihlašovací údaje do JSON souboru.

Rozhraní vychází z Material Design 3 a obsahuje čtyři obrazovky: Přehled, Nová transakce, Kurzy měn a Nastavení.

Využití AI

- Při vývoji byl využit AI asistent Claude (Anthropic) prostřednictvím Claude Code, především pro:
- Návrh a úpravu struktury projektu.
- Vytvoření uživatelského rozhraní v XAML a designového systému.
- Tvorbu ikon a grafických prvků.
- Implementaci navigace mezi stránkami.
- Pomoc s přepínáním světlého a tmavého motivu.

Vytvoření dokumentace README.

Samostatně jsem implementoval programovou logiku, zejména validaci formulářů, ukládání transakcí, výpočty a filtrování na hlavní obrazovce, kontrolu síly hesla, práci s JSON souborem a ošetření chyb.