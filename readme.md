# Swordsman Game
( tymczasowy tytuł roboczy )

## Ogólny opis:
Gra napisana w środowisku __Unity__, korzystając z __C#__, oraz silnika __2D__.  
__Przygodowa Gra Akcji__ osadzona w nieco pomieszanym świecie z Futurystycznym klimatem pomieszanym z krwawymi bestiami i widmem apokalipsy.
Gra będzie charakteryzować się nieszablonowym podejściem do gameplayu, jednak z kilkoma utartymi mechanikami.

### Gameplay:
Gracz dzierży jedną główną broń, jednak może zmieniać jej typ obrażeń w zależności od buildu i preferencji (**Flame**, **Storm**, **Ice**, **Bloody**, **Normal**).  
Dodatkowo będzie kładzony nacisk na zbieranie losowych przedmiotów z pasywnymi i aktywnymi umiejętnościami definiującymi styl gry.  
Przedmioty, oraz statystyki postaci będzie można ulepszać za pomocą dusz demonów i złota (niektóre ulepszenia będą dostępne za pewnym pułapem poziomu gracza).  
Przedmioty i przeciwnicy będą mieli losowe statystyki (mała różnica między górną i dolną granicą), wzmacniane dodatkowo przez klasę:  
- Noob (kolorwa) występuje tylko na niskim poziomie trudności przeznaczonym dla klaunów (śmieszna wariacja niskiego poziomu trudności, wyśmiewająca nieco graczy którzy lubią na niskim grać, giga silne przedmioty i słabi przeciwnicy)
- Legendarna (złota)
- Epicka (fioletowa)
- Rzadka (jasno-niebieska)
- Normalna (bez koloru)

### Przeciwnicy: 
Przeciwnicy będą dość agresywni, lecz ich modele będą zawierać słabe punkty, gdzie za zadawanie obrażenia w nie gracz będzie nagradzany dodatkowymi obrażeniami w te punkty.  
Kazdy z nich powinien wymagać odrębnego podejścia do pokonania go, co będzie uwarunkowane przez rozmieszczenie słabych punktów, czy statystyki i słabości.  

W grze będzie zawarta mechanika "gnojenia gracza":
- Przeciwnik, który pokona gracza (oprócz bossów), będzie podnosił swoją klasęi zbierał expa zebranego z gracza
- Ulepszony przeciwnik dropi lepsze przedmioty i dodatkowy exp
- Maksymalny poziom takiego przeciwnika to Noobski (gracz, który zginie o Legendarnego przeciwnika, osłabia go na poziom Noob, który osłabia przeciwników)
