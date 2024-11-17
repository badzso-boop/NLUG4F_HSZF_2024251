# **Háztartási Menedzsment Alkalmazás**

Ez az alkalmazás egy háztartás kezelésére szolgáló rendszer, amely lehetővé teszi a hűtőszekrény és a kamra tartalmának kezelését, események alapján történő reagálást, CRUD műveletek végrehajtását, ételek elkészítését, és vásárlás szimulálását.

## **Projektek**

A megoldás 5 különálló projektre van felosztva:
1. **Application**: Az alkalmazás üzleti logikája és funkcionalitása.
2. **Model**: Az adatmodellek definiálása.
3. **MsSql**: Az adatbázis kapcsolódás és kezelés.
4. **Console**: Felhasználói interakciók CLI környezetben.
5. **Tests**: Tesztelési projekt az alkalmazás funkcióinak ellenőrzésére.

---

## **Application Projekt**

Az **Application** projekt tartalmazza az üzleti logikát és a különböző adatkezelési műveleteket.

### **Fájlok és Funkciók**

#### **1. Events**
- **Funkció**: Az alkalmazás működése során előforduló események kezelése.
- **Részletek**:
  - Publikus események deklarálása.
  - Események kiváltása más komponensek által.

#### **2. FridgeCRUD**
- **Funkció**: A hűtőszekrényhez kapcsolódó CRUD műveletek kezelése.
- **Metódusok**:
  - `KiirAll`: Az összes hűtőben lévő termék listázása.
  - `KiirEgy`: Egy adott termék megjelenítése.
  - `Hozzaad`: Új termék hozzáadása.
  - `Update`: Egy meglévő termék frissítése.
  - `Delete`: Egy termék törlése.
- **Konstruktor paraméterei**:
  - `HouseHoldDBContext`
  - `FridgeDataProvider`
  - `ProductDataProvider`

#### **3. Interfaces**
- **Funkció**: A CRUD műveletek interfészeinek deklarálása, amelyeket a megfelelő osztályok implementálnak.
- **Metódusok**:
  - `KiirAll`, `KiirEgy`, `Hozzaad`, `Update`, `Delete` (mind `void` típusú).

#### **4. MakeFood**
- **Funkció**: Ételkészítési folyamatok és a kapcsolódó adatbázis műveletek kezelése.
  - Felhasználói interakció vezérelt folyamat.
  - Az ételkészítéshez szükséges összetevők eltávolítása vagy frissítése.
- **Konstruktor paraméterei**:
  - `HouseHoldDBContext`
  - `ProductCRUD`
  - `ProductDataProvider`

#### **5. PantryCRUD**
- **Funkció**: A kamra tartalmának kezeléséhez kapcsolódó CRUD műveletek.
- **Metódusok**:
  - `KiirAll`: Az összes kamrában lévő termék listázása.
  - `KiirEgy`: Egy adott termék megjelenítése.
  - `Hozzaad`: Új termék hozzáadása.
  - `Update`: Egy meglévő termék frissítése.
  - `Delete`: Egy termék törlése.
- **Konstruktor paraméterei**:
  - `HouseHoldDBContext`
  - `PantryDataProvider`
  - `ProductDataProvider`

#### **6. PersonsCRUD**
- **Funkció**: A személyek kezeléséhez kapcsolódó CRUD műveletek.
- **Metódusok**:
  - `KiirAll`: Az összes személy listázása.
  - `KiirEgy`: Egy adott személy megjelenítése.
  - `Hozzaad`: Új személy hozzáadása.
  - `Update`: Egy meglévő személy frissítése.
  - `Delete`: Egy személy törlése.
- **Konstruktor paraméterei**:
  - `HouseHoldDBContext`
  - `PersonDataProvider`

#### **7. ProductCRUD**
- **Funkció**: A termékek kezeléséhez kapcsolódó CRUD műveletek.
- **Metódusok**:
  - `KiirAll`: Az összes termék listázása.
  - `KiirEgy`: Egy adott termék megjelenítése.
  - `Hozzaad`: Új termék hozzáadása.
  - `Update`: Egy meglévő termék frissítése.
  - `Delete`: Egy termék törlése.
- **Konstruktor paraméterei**:
  - `HouseHoldDBContext`
  - `ProductDataProvider`

#### **8. Querry**
- **Funkció**: Adatbázis-lekérdezések végrehajtása és adatok exportálása.
- **Metódusok**:
  - `GetAllStockProduct()`: Az összes raktáron lévő termék lekérdezése.
  - `GetLowStockItems()`: Az alacsony készleten lévő termékek lekérdezése.
  - `GetExpiringSoon()`: A hamarosan lejáró termékek listázása.
  - `ExportToTxt()`: Adatok exportálása szöveges fájlba.

#### **9. Shopping**
- **Funkció**: A vásárlás szimulációját kezeli.
  - Új termékek hozzáadása a hűtő vagy a kamra adatbázisába.
- **Metódusok**:
  - Vásárlási műveletek szimulációja.
  - A termékek megfelelő helyre történő hozzáadása.

---

## **Application Projekt**

## **Fájlok és Funkciók**

### **1. Fridge.cs**
- **Funkció**: A hűtőszekrény adatmodellje.
- **Tulajdonságok**:
  - `Id` (int): Egyedi azonosító (adatbázis által generált).
  - `Capacity` (int): A hűtő maximális kapacitása.
  - `Products` (List<Product>): A hűtőben tárolt termékek listája.
- **Konstruktorok**:
  - `Fridge(int capacity)`: Kapacitás megadásával hoz létre egy üres hűtőt.
  - `Fridge(int capacity, List<Product> productsToAdd)`: Kapacitással és kezdeti terméklistával hoz létre egy hűtőt.
  - `Fridge()`: Üres konstruktor alapértelmezett értékekkel.

---

### **2. Pantry.cs**
- **Funkció**: A kamra adatmodellje.
- **Tulajdonságok**:
  - `Id` (int): Egyedi azonosító (adatbázis által generált).
  - `Capacity` (int): A kamra maximális kapacitása.
  - `Products` (List<Product>): A kamrában tárolt termékek listája.
- **Konstruktorok**:
  - `Pantry(int capacity)`: Kapacitás megadásával hoz létre egy üres kamrát.
  - `Pantry(int capacity, List<Product> productsInput)`: Kapacitással és kezdeti terméklistával hoz létre egy kamrát.
  - `Pantry()`: Üres konstruktor alapértelmezett értékekkel.

---

### **3. Persons.cs**
- **Funkció**: A személyek adatmodellje.
- **Tulajdonságok**:
  - `Id` (int): Egyedi azonosító (adatbázis által generált).
  - `Name` (string): A személy neve (max. 100 karakter).
  - `ResponsibleForPurchase` (bool): Jelzi, hogy a személy felelős-e a vásárlásokért.
  - `FavoriteProductIds` (List<int>): A személy kedvenc termékeinek azonosítói.
- **Konstruktorok**:
  - `Person(string name, bool responsibleForPurchase)`: Létrehoz egy személyt névvel és felelősségi állapottal.
  - `Person(string name, bool responsibleForPurchase, List<int> favoriteProductIdsInput)`: Névvel, felelősségi állapottal és kedvenc termékek listájával hoz létre egy személyt.
  - `Person()`: Üres konstruktor alapértelmezett értékekkel.

---

### **4. Products.cs**
- **Funkció**: A termékek adatmodellje.
- **Tulajdonságok**:
  - `Id` (int): Egyedi azonosító (adatbázis által generált).
  - `Name` (string): A termék neve (max. 100 karakter).
  - `Quantity` (decimal): A termék mennyisége.
  - `CriticalLevel` (decimal): Az alacsony készletszintet jelző érték.
  - `BestBefore` (DateTime): A termék lejárati dátuma.
  - `StoreInFridge` (bool): Jelzi, hogy a terméket hűtőben kell-e tárolni.
  - **Navigációs Tulajdonságok**:
    - `FridgeId` (int?): A termékhez tartozó hűtő azonosítója.
    - `Fridge` (Fridge): A termékhez tartozó hűtő objektum.
    - `PantryId` (int?): A termékhez tartozó kamra azonosítója.
    - `Pantry` (Pantry): A termékhez tartozó kamra objektum.
- **Konstruktorok**:
  - `Product(string name, decimal quantity, decimal criticalLevel, DateTime bestBefore, bool storeInFridge)`: Termék létrehozása a szükséges adatokkal.
  - `Product()`: Üres konstruktor alapértelmezett értékekkel.

---

### **5. ProductInterfaces.cs**
- **Funkció**: Interfész a termékekkel kapcsolatos műveletekhez.
- **Metódusok**:
  - `GetExpiringSoon()`: Lekérdezi a hamarosan lejáró termékeket.
  - `GetLowStockItems()`: Lekérdezi az alacsony készleten lévő termékeket.
  - `GetAllStockProduct()`: Lekérdezi az összes raktáron lévő terméket.

---

### **6. MainInterfaces.cs**
- **Funkció**: Generikus interfész az alapvető CRUD műveletekhez.
- **Metódusok**:
  - `Add(T entity)`: Új entitás hozzáadása.
  - `Update(T entity)`: Egy meglévő entitás frissítése.
  - `Delete(int id)`: Egy entitás törlése azonosító alapján.
  - `GetById(int id)`: Egy entitás lekérdezése azonosító alapján.
  - `GetAll()`: Az összes entitás lekérdezése.

---

## **Adatmodell Kapcsolatok**

- **Termékek és Tárolók**:
  - A termékek hűtőben vagy kamrában tárolhatók, navigációs tulajdonságok segítségével kapcsolódnak hozzájuk.
- **Személyek és Termékek**:
  - A személyek kedvenc termékeiket az `FavoriteProductIds` tulajdonságon keresztül definiálják.

---

## MsSQL

---

## Funkciók

1. **Entitásmodellek**:
   - **Person**: A háztartás tagjait reprezentálja, beleértve a vásárlásért felelős személyeket és kedvenc termékeket.
   - **Product**: A háztartásban tárolt termékeket ábrázolja, beleértve a kritikus készletszinteket és lejárati időket.
   - **Fridge**: A romlandó termékek tárolására szolgáló hűtőszekrény kapacitáskorláttal.
   - **Pantry**: Nem romlandó termékek tárolására szolgáló kamra kapacitáskorláttal.

2. **Adatbázis-kapcsolat**:
   - Konfigurálja a kapcsolatokat a `Fridge`, `Pantry` és `Product` között.
   - MSSQL adatbázis-kapcsolatot hoz létre.

3. **Adatszolgáltatók**:
   - CRUD műveletek megvalósítása minden entitásra (`Person`, `Product`, `Fridge`, `Pantry`).
   - Adatellenőrzés és egyedi kivételek dobása érvénytelen műveletek esetén.
   - Események kezelése, például alacsony készletszintek figyelése.

4. **JSON-integráció**:
   - Alapadatok beolvasása és feldolgozása egy JSON fájlból.

5. **Egyedi kivételek**:
   - Részletes hibaüzenetek az érvénytelen adatokra és az entitások hiányára vonatkozóan.

---

## Beállítás és Konfiguráció

### 1. Előfeltételek
- .NET 6.0 SDK vagy újabb verzió.
- MSSQL Server (LocalDB vagy teljes SQL Server).
- JSON adatfájl, amely a `jsons/data.json` mappában található.

### 2. Kapcsolati karakterlánc
A kapcsolati karakterlánc a `HouseHoldDbContext.cs` fájlban van definiálva:

```csharp
string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=house";
```

---

## Entitásosztályok

### 1. **Product**
A háztartásban tárolt termékeket ábrázolja.

- **Tulajdonságok**:
  - `Name`: A termék neve (kötelező, legfeljebb 100 karakter).
  - `Quantity`: A termék mennyisége (nem lehet negatív).
  - `CriticalLevel`: Kritikus készletszint, amely alatt figyelmeztetést ad (nem lehet negatív).
  - `BestBefore`: A termék lejárati dátuma (jövőbeli dátum kell, hogy legyen).
  - `StoreInFridge`: Logikai érték, amely megadja, hogy a terméket hűtőben kell-e tárolni.

---

### 2. **Person**
A háztartás tagjait ábrázolja.

- **Tulajdonságok**:
  - `Name`: A személy neve (kötelező, legfeljebb 100 karakter).
  - `ResponsibleForPurchase`: Logikai érték, amely megadja, hogy a személy felelős-e a vásárlásokért.
  - `FavoriteProductIds`: Kedvenc termékek listája (egész számok listája).

---

### 3. **Fridge**
A hűtőszekrényt reprezentálja, amely korlátozott kapacitással rendelkezik.

- **Tulajdonságok**:
  - `Capacity`: A hűtő kapacitása (kötelező, nem lehet negatív).
  - `Products`: A hűtőben tárolt `Product`-ok listája.

---

### 4. **Pantry**
A kamrát reprezentálja, amely szintén korlátozott kapacitással rendelkezik.

- **Tulajdonságok**:
  - `Capacity`: A kamra kapacitása (kötelező, nem lehet negatív).
  - `Products`: A kamrában tárolt `Product`-ok listája.

---

## Adatszolgáltatók

### 1. **ProductDataProvider**
A `Product` entitások CRUD műveleteinek kezelésére szolgál, és eseményeket vált ki alacsony készletszintek esetén.

- **Metódusok**:
  - `Add(Product entity)`: Új termék hozzáadása.
  - `Update(Product entity)`: Meglévő termék frissítése.
  - `Delete(int id)`: Termék törlése azonosító alapján.
  - `GetById(int id)`: Termék lekérése azonosító alapján.
  - `GetAll()`: Az összes termék lekérése.

- **Esemény**:
  - `ProductBelowCriticalLevel`: Akkor váltódik ki, ha egy termék készlete a kritikus szint alá csökken.

---

### 2. **PersonDataProvider**
A `Person` entitások CRUD műveleteinek kezelésére szolgál.

- **Metódusok**:
  - `Add(Person entity)`: Új személy hozzáadása.
  - `Update(Person entity)`: Meglévő személy frissítése.
  - `Delete(int id)`: Személy törlése azonosító alapján.
  - `GetById(int id)`: Személy lekérése azonosító alapján.
  - `GetAll()`: Az összes személy lekérése.

---

### 3. **FridgeDataProvider**
A `Fridge` entitások CRUD műveleteinek kezelésére szolgál.

- **Metódusok**:
  - `Add(Fridge entity)`: Új hűtő hozzáadása.
  - `Update(Fridge entity)`: Meglévő hűtő frissítése.
  - `Delete(int id)`: Hűtő törlése azonosító alapján.
  - `GetById(int id)`: Hűtő lekérése azonosító alapján.
  - `GetAll()`: Az összes hűtő lekérése.

---

### 4. **PantryDataProvider**
A `Pantry` entitások CRUD műveleteinek kezelésére szolgál.

- **Metódusok**:
  - `Add(Pantry entity)`: Új kamra hozzáadása.
  - `Update(Pantry entity)`: Meglévő kamra frissítése.
  - `Delete(int id)`: Kamra törlése azonosító alapján.
  - `GetById(int id)`: Kamra lekérése azonosító alapján.
  - `GetAll()`: Az összes kamra lekérése.

---

## Egyedi Kivételek

- **InvalidProductDataException**: Akkor dobódik, ha egy termék adatai érvénytelenek.
- **ProductNotFoundException**: Akkor dobódik, ha egy termék nem található az adatbázisban.
- **InvalidPersonDataException**: Akkor dobódik, ha egy személy adatai érvénytelenek.
- **PersonNotFoundException**: Akkor dobódik, ha egy személy nem található az adatbázisban.

---

## Használati Példák

### 1. Adatfeltöltés
A `JsonRead` osztály segítségével tölthetjük be az adatokat a JSON fájlból:
```csharp
var jsonRead = new JsonRead(context);
jsonRead.SeedDatabase(context);
```

### 2. Eseménykezelés
```csharp
productDataProvider.ProductBelowCriticalLevel += (sender, args) =>
{
    Console.WriteLine($"A(z) '{args.Product.Name}' készletszintje kritikus alá csökkent! Értesítendő: {args.Person.Name}.");
};
```

### 3. CRUD műveletek
```csharp
var newProduct = new Product("Tej", 1.0M, 0.5M, DateTime.Now.AddDays(7), true);
productDataProvider.Add(newProduct);
```
---

## UnitTests

# PersonDataProviderTests

Ez az osztály a `PersonDataProvider` működésének tesztelésére készült. A tesztek biztosítják, hogy az `Person` entitások CRUD műveletei és hibakezelése megfelelően működjenek.

---

## Tesztelt funkciók

### 1. `AddPerson`
- Ellenőrzi, hogy egy személy sikeresen hozzáadható az adatbázishoz.
- A teszt után ellenőrzi, hogy az adatbázis tartalmazza az újonnan hozzáadott személyt.

### 2. `GetAllPeople`
- Ellenőrzi, hogy az összes hozzáadott személy lekérhető.
- Több személy hozzáadása után biztosítja, hogy a lekérdezés visszaadja az összes személyt.

### 3. `GetPersonById`
- Ellenőrzi, hogy egy konkrét személy azonosító alapján sikeresen lekérhető.
- Biztosítja, hogy a lekért személy adatai helyesek.

### 4. `UpdatePerson`
- Ellenőrzi, hogy egy meglévő személy adatai frissíthetők.
- A frissítés után biztosítja, hogy az adatbázisban tárolt értékek megfelelően változtak.

### 5. `DeletePerson`
- Ellenőrzi, hogy egy személy sikeresen törölhető az adatbázisból.
- A törlés után biztosítja, hogy az adatbázisban már ne legyen jelen az adott személy.

---

## Hibakezelési tesztek

### 1. `Add_Error`
- Ellenőrzi, hogy egy érvénytelen személy hozzáadása kivételt vált ki (`InvalidProductDataException`).

### 2. `Update_Error`
- Ellenőrzi, hogy érvénytelen adatokkal történő frissítés kivételt vált ki (`InvalidProductDataException`).

### 3. `GetById_Error`
- Ellenőrzi, hogy nem létező azonosítóval történő lekérés kivételt vált ki (`PersonNotFoundException`).

### 4. `Update_Error404`
- Ellenőrzi, hogy nem létező személy frissítése kivételt vált ki (`PersonNotFoundException`).

### 5. `Delete_Error`
- Ellenőrzi, hogy nem létező azonosítóval történő törlés kivételt vált ki (`PersonNotFoundException`).

---

## Setup
A tesztek a következő objektumokat inicializálják a tesztosztályban:
- `HouseHoldDbContext`: Az adatbázis kapcsolata.
- `PersonDataProvider`: A `Person` entitások kezeléséhez.
- `ProductDataProvider`: A kapcsolódó termékkezeléshez.
- `PersonCRUD`: A CRUD műveletek magasabb szintű kezelésére.

A tesztek minden futtatás előtt újra inicializálják az adatbázist.

---

# ProductDataProviderTests

Ez az osztály a `ProductDataProvider` működésének tesztelésére készült. A tesztek biztosítják, hogy a `Product` entitások CRUD műveletei és hibakezelése megfelelően működjenek.

---

## Tesztelt funkciók

### 1. `AddProduct`
- Ellenőrzi, hogy egy termék sikeresen hozzáadható az adatbázishoz.
- A teszt után biztosítja, hogy az adatbázis tartalmazza az újonnan hozzáadott terméket.

### 2. `GetProductById`
- Ellenőrzi, hogy egy konkrét termék azonosító alapján sikeresen lekérhető.
- Biztosítja, hogy a lekért termék adatai helyesek.

### 3. `UpdateProduct`
- Ellenőrzi, hogy egy meglévő termék adatai frissíthetők.
- A frissítés után biztosítja, hogy az adatbázisban tárolt értékek megfelelően változtak.

### 4. `DeleteProduct`
- Ellenőrzi, hogy egy termék sikeresen törölhető az adatbázisból.
- A törlés után biztosítja, hogy az adatbázisban már ne legyen jelen az adott termék.

### 5. `GetAllProducts`
- Ellenőrzi, hogy az összes hozzáadott termék lekérhető.
- Több termék hozzáadása után biztosítja, hogy a lekérdezés visszaadja az összes terméket.

---

## Hibakezelési tesztek

### 1. `Add_Error`
- Ellenőrzi, hogy érvénytelen termék hozzáadása kivételt vált ki (`InvalidProductDataException`).

### 2. `GetById_Error`
- Ellenőrzi, hogy nem létező azonosítóval történő lekérés kivételt vált ki (`ProductNotFoundException`).

### 3. `Update_Error`
- Ellenőrzi, hogy érvénytelen adatokkal történő frissítés kivételt vált ki (`InvalidProductDataException`).

### 4. `Delete_Error`
- Ellenőrzi, hogy nem létező azonosítóval történő törlés kivételt vált ki (`ProductNotFoundException`).

---

## Setup
A tesztek a következő objektumokat inicializálják a tesztosztályban:
- `HouseHoldDbContext`: Az adatbázis kapcsolata.
- `ProductDataProvider`: A `Product` entitások kezeléséhez.
- `PersonDataProvider`: A kapcsolódó személykezeléshez.

A tesztek minden futtatás előtt újra inicializálják az adatbázist.

---

## Console application

## Funkciók

### Főbb Funkciók
1. **Lekérdezések**
   - **Közelgő lejáratú termékek**: Azokat a termékeket listázza, amelyek lejárata hamarosan esedékes.
   - **Kifogyóban lévő termékek**: Azokat a termékeket jeleníti meg, amelyek elérték vagy alatta vannak a kritikus készlet szintnek.
   - **Maradék összkapacitás**: Az összes raktárkészlet állapotát összegzi.

2. **Étel készítése**
   - Segíti a felhasználót az ételkészítésben, az elérhető háztartási alapanyagok alapján.

3. **Bevásárlás**
   - Automatikusan pótolja a termékeket a kritikus szint vagy a háztartás tagjainak kedvenc termékei alapján.

4. **Adatbeolvasás**
   - A JSON fájlból adatokat olvas be az adatbázis kezdeti feltöltéséhez.

5. **Adatkiírás**
   - Az adatokat egy szöveges fájlba exportálja biztonsági mentés vagy további felhasználás céljából.

6. **Adatbázis-kezelés**
   - CRUD (Létrehozás, Olvasás, Frissítés, Törlés) műveleteket végezhet a következő entitásokon:
     - Termékek
     - Személyek
     - Hűtők
     - Kamrák

---

## Események és Riasztások
1. **Kritikus szint alatti termékek**
   - Értesíti a felhasználót, amikor egy termék készlete a kritikus szint alá esik.
   - Tartalmazza a felelős személy adatait, aki a termék pótlásáért felelős.

2. **Kedvenc termék pótolva**
   - Riaszt, amikor egy háztartási tag kedvenc terméke pótolva lett.

3. **Kritikus szint alatti termékek lekérdezése**
   - A rendszer riasztást küld, ha több termék is elérte a kritikus készletszintet.

---

## Menü Struktúra
Az alkalmazás hierarchikus menürendszert használ, amely lehetővé teszi a felhasználók számára, hogy könnyen navigáljanak a különböző funkciók között.

### Főmenü Opciók
1. **Lekérdezések**
   - Almenü tartalmazza:
     - Közelgő lejáratú termékek
     - Kifogyóban lévő termékek
     - Maradék összkapacitás

2. **Étel készítése**
   - Segíti az ételkészítést a háztartás alapanyagai alapján.

3. **Bevásárlás**
   - Pótlja a termékeket a szükségletek alapján.

4. **Beolvasás**
   - JSON fájlból történő adatimportálás.

5. **Kiírás**
   - Adatok exportálása szöveges fájlba.

6. **DB Szerkesztés**
   - Almenü tartalmazza a CRUD műveleteket:
     - Termékek
     - Személyek
     - Hűtők
     - Kamrák

7. **Kilépés**
   - Kilépés az alkalmazásból.

---

## Használat
1. Indítsa el az alkalmazást.
2. Navigáljon a menüben az adott számjegy megadásával.
3. Kövesse a képernyőn megjelenő utasításokat a műveletek végrehajtásához vagy térjen vissza a főmenübe.
4. A DB Szerkesztés menüpontban adhat hozzá, frissíthet vagy törölhet adatokat a háztartásról.

---

## Hibakezelés
- **Érvénytelen adat**: Hibás adatbevitel esetén megfelelő hibaüzenetek jelennek meg.
- **Nem található adat**: Ha nem létező rekordra próbál adatot módosítani vagy megjeleníteni, a rendszer értesíti a felhasználót.

---

## Függőségek
- Az alkalmazás adatkezeléshez a `HouseHoldDbContext` adatbázis-kontektszt használ.
- JSON fájl szükséges az adatbázis kezdeti feltöltéséhez.

---

## Események
- **ProductBelowCriticalLevel**: Esemény akkor aktiválódik, amikor egy termék készlete a kritikus szint alá esik.
- **FavoriteProductRestock**: Esemény akkor aktiválódik, amikor egy háztartási tag kedvenc terméke pótolva lett.
- **ProductsBelowCriticalLevel**: Esemény akkor aktiválódik, amikor több termék készlete is alá esett a kritikus szintnek.

---

## Megjegyzés
- A menürendszer interaktív, és minden művelet után visszaáll a főmenübe.
- Az események és műveletek megfelelő kezelése biztosítja a zökkenőmentes működést és a valós idejű frissítéseket.

Élvezze a háztartás hatékony kezelését ezzel az alkalmazással!

---

## **Használati Utasítás**

1. Telepítsd a szükséges csomagokat a projekt buildelése előtt.
2. Futtasd az adatbázis inicializálási scriptet az MsSql projektben.
3. Használj `Console` projektet a CLI interakciók eléréséhez.

## **Tesztelés**

A `Tests` projekt tartalmazza az összes funkció tesztelésére szolgáló teszteseteket. A tesztek futtatásához használd az MSTest frameworköt.

---