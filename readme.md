﻿# Neo Punk

## Ogólny opis:
Gra napisana w środowisku __Unity__, korzystając z __C#__, oraz silnika __2D__.  
__Przygodowa Gra Akcji__ osadzona w nieco pomieszanym świecie z Futurystycznym klimatem pomieszanym z krwawymi bestiami i widmem apokalipsy.
Gra będzie charakteryzować się nieszablonowym podejściem do gameplayu, jednak z kilkoma utartymi mechanikami.

## Gameplay:
Gracz dzierży jedną główną broń, jednak może zmieniać jej typ obrażeń w zależności od buildu i preferencji (**Flame**, **Storm**, **Water**, **Air**, **Bloody**, **Normal**).  
Dodatkowo będzie kładzony nacisk na zbieranie losowych przedmiotów z pasywnymi i aktywnymi umiejętnościami definiującymi styl gry.  
Przedmioty, oraz statystyki postaci będzie można ulepszać za pomocą złota (niektóre ulepszenia będą dostępne za pewnym pułapem poziomu gracza).  
**Charakterystyka poszczególnych typów obrażeń:**

### - Flame
Efektywny przeciw: przeciwnikom ludzkim, oraz demonicznym bez cybernetycznych elementów  
Słabość: każdy przeciwnik w pełni cybernetyczny  
Cechy szczególne: dość powszechny element, nakładany tkże przez każdą broń palną
### - Storm
Efektywny przeciw: wszystkiemu co posiada elektronikę  
Słabość: przeciwicy demoniczni  
Cechy szczególne: element ten jest szczególnie niebezpieczny dla elektroniki,  
pozwala na niszczenie niektórych elektronicznych elementów otoczenia
### - Water
Efektywny przeciw: wszystkiemu co posiada elektronikę  
Słabość: ludzie  
Cechy szczególne: element ten jest szczególnie niebezpieczny dla elektroniki,  
pozwala na niszczenie niektórych elektronicznych elementów otoczenia
### - Air
Efektywny przeciw: ludzkiemu ciału  
Słabość: przeciwnicy w pełni cybernetyczni  
Cechy szczególne: gracz dzierżący ten element ma delikatnie zwiększoną prędkość ruchu, oraz ataku
### - Bloody
Efektywny przeciw: każdemu demonowi, oraz każdemy człowiekowi  
Słabość: przeciwnicy w pełni cybernetyczni  
Cechy szczególne: element szczególnie potężny, jednak nie można go wybrać tak jak pozostałe elementy  
można go osiągnąć jedynie poprzez przedmioty i ich efekty
### - Normal
Efektywny przeciw: niczemu  
Słabość: przeciwnicy w pełni cybernetyczni  
Cechy szczególne: raczej ogólny element pozwalający na większą elastyczność działań

Gra będzie także glorfikować przechodzenie jej ponownie i do tego zachęcać, poprzez różne style jakimi można się w niej bawić.
Będzie służyć także jako piaskownica i powinna dawać pole do popisu speedrunnerom(podbija to jej popularność i żywotność).

## Przeciwnicy: 
Kazdy z przeciwników powinien wymagać odrębnego podejścia do pokonania go, co będzie uwarunkowane przez jego ataki, czy statystyki i słabości.  

W grze będzie zawarta mechanika **"gnojenia gracza"**:
- Przeciwnik, który pokona gracza (wykluczając bossy), będzie podnosił swój poziom i zbierał całe złoto podniesione po śmierci gracza, oraz jeden losowy przedmiot z ekwipunku gracza.
- Jeśli gracz ponownie zginie od takiego pzeciwnika, to traci zebrane przez niego złoto bezpowrotnie, lecz przeciwnik zostaje w ulepszonym stanie.
- Po śmierci gracza każdy przeciwnik na aktualnie przechodzonym poziomie się odradza.

Przeciwnicy będą rozstawieni po mapie ręcznie, ale także będą się spawnować podczas "inwazji".  
Czyli opcjonalnej areny gdzie gracz walczy z falami przciwników. Będą one ułożone tak by były rzadkie(ale nie za bardzo) i dropiły przedmnioty po pokonaniu inwazji.

## Przedmioty:
Każdy przedmiot powinien mieć umiejętność pasywną i aktywną, no i powinien dodawać małe ciekawe mechaniki, jak dashe, kotwiczki, teleportacje, uniki itp.
Gameplay będzie definiowany poprzez posiadane przez gracza przedmioty, które będą się pojawiać po zakończonej inwazji.
[Lista przedmiotów](docs/items.md)

## Lore:
Lore w zamyśle będzie podawany poprzez dialogi i wydarzenia w tle. 
Historia będzie dość przejrzysta, lecz, aby zwiększyć nieco zainteresowanie i zaangażowanie, pewne jej elementy, będą niejawne, dając graczom pewne, lecz ograniczone pole do interpretacji.
[Dokładniejszy opis lore](docs/lore.md)

## Roadmap:
1. [x] Podstawowe animacje gracza, poruszanie się i walka
2. [x] Podstawowy przeciwnik, do testów
3. [x] Pierwsze w pełni zaimplementowane przedmioty (na okres tutoriala 4)
4. [x] Przebudowa ekwipunku
5. [x] Podstawowi przeciwnicy (3: demon, człowiek, robot)
6. [x] Lokalna baza danych zawierająca podpowiedzi, oraz mechanika wyświetlania podpowiedzi.
7. [x] Pierwszy poziom (tutorial)
8. [ ] rozbudowa startowego poziomu
9. [ ] bugfixy
10. [ ] odświeżenie głównej postaci
11. [ ] odświeżenie menu głównego
12. [ ] muzyka i udźwiękowienie
13. [ ] testy 
