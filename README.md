# ServerLogin
Serwer z funkcją logowania i dodawania nowych użytkowników.

## Wymagania funkcjonalne
- Łączenie z serwerem poprzez PuTTy. Serwer wysyła wiadomość z instrukcją do klienta i czeka na odpowiedź.
- Możliwe działania to logowanie i wylogowanie, dodanie nowego użytkownika, rozłączenie.
- Dane użytkowników przechowywane są w pliku.

## Wymagania pozafunkcjonalne
- Aplikacja serwera ma postać aplikacji konsolowej systemu Windows.
- Komunikacja polega na przesyłaniu surowych danych (bajtów)
- W ramach serwera zaimplementowana jest implementowana obsługa rozłączającego się klienta.
- Serwer może utrzymać wiele połączeń, ale nie posiada zabezpieczenia przed logowaniem się jako ten sam użytkownik w tym samym momencie.
- Serwer po zakończeniu połączenia czeka na kolejne połączenia.
- Serwer pobiera bazę użytkowników z pliku i zapisuje nowych do tego pliku.
- W razie wystąpienia błędu serwer uruchamia się ponownie.
- Serwer wymaga .NET Framework w wersji 4.0 lub nowszej.
