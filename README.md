# IAU_GraduationProject **`Unity 2022.3.13f1`**

## İçindekiler

### 1. [Değişken İsimlendirme](#değişken-isimlendirme)
### 2. [Yorum Satırları ve Açıklamalar (// /**/)](#yorum-satırları)
### 3. [Bölümleme (#region #endregion)](#bölümleme)
### 4. [Koşullu Derleme (#if-#endif)](#koşullu-derleme)
### 3. [Yardımcı Metotlar (Extentions)](#yardımcı-metotlar)
### 4. [Kod Şablonları (Script Templates)](#kod-şablonları)

## Kod Yazma Kuralları

### Değişken İsimlendirme 

Değişkenin **Public**, **Private** veya **Local** bir değişken mi olduğunu anlamamızı sağlayacak bunun sayesinde bir **alanda (scope)** o değişkeni kullanıp kullanamayacağımızı anlayabilir bakmak için geri dönmeye gerek duymaz ve zaman kaybetmeyiz.

#### Değişken
| Değişken Türü                       | Başlangıç       | Örnek               |
|:------------------------------------|:----------------|:--------------------|      
| Private                             | _ ve Küçük Harf | int _myHealth       |          
| Public                              | Büyük Harf      | int Health          |         
| Local                               | Küçük Harf      | int myHealth        |

#### Sınıf
| Değişken Türü                       | Başlangıç       | Örnek               |
|:------------------------------------|:----------------|:--------------------| 
| Class                               | Büyük Harf      | Class MyClass       |          

#### Metot
| Değişken Türü                       | Başlangıç       | Örnek               |
|:------------------------------------|:----------------|:--------------------| 
| Metot                               | Büyük Harf      | public void Metot() | 

<!--|                                     |                 |                     |--> 

### Yorum Satırları ve Açıklamalar

Yorum satırları yazdığımız kodun ne işe yaradığını anlatmak,
kod dosyalarının başına isim yazmak için kullanılır.

Metotların başına tekli yorum satırı için `// Yorum` veya çoklu yorum satırı için `/* Yorum */` şeklinde yazıp açıklama yapabiliriz.

``` 
// Bu bir tekli yorum satırıdır.

/*
   Bu bir çoklu
   yorum satırıdır.
*/
```

Sınıflara, değişkenlere ve metotlara açıklama eklemek için kullanılır.
`///` şeklinde yazıdığımızda oluşan `summary` etiketlerinin arasına açıklamamızı ekleyebiliriz. <br>
Bir metoddaki değişkenlere açıklama ekleyebiliz. <br>
Metodun ne döndüreceği ile ilgili açıklama yazabiliriz. <br>


```


/// <summary>
/// Verilen sayıyı 5 ile çarparak döndürür
/// <!-- Açıklama içine yorum satırı -->
/// </summary>
/// <param name="a"> Bir tamsayı </param> 
/// <returns> a * 5 </returns>
public int AciklamaliMetot(int a)
{
   return a * 5;
}
```
> [!IMPORTANT]
> [Daha Fazla Etiket][1]

### Bölümleme (Regions) 

**Regions** kodu bölümlere ayırmaya sağlar. <br> 
Bölümün başına `#region` yazdıktan sonra boşluk bırakarak bölüm başlığı yazılır ve bölümün bittiği yere `#endregion` eklenir.
```
#region Karakter Hareket
public void Movement()
{
}
#endregion

#region Karakter Saldırı
public void Attack()
{
}
#endregion
```

### Koşullu Derleme

Test kodlarının sadece **Unity Editörde** derlenmesini sağlamak için koşullar ekleyebiliriz.
Sadece telefonda, windows veya linux vb. platformlarda çalışan kodlar yazabilirsiniz.
Başına `#if Platform_Sembolü` ve sonuna `#endif` yazarak bunu sağlayabiliriz.

```
void Update()
{
   // Sadece Unity içinde çalışır
   #if UNITY_EDITOR 
      Debug.Log("TEST DEBUG");
   #endif
}
```

> [!IMPORTANT]
> [Diğer Platform Sembolleri][2]

### Yardımcı Metotlar (Extentions)

Kodlama sürecini hızlandırmak için kullandığımız genel kullanıma sahip **Static** sınıflar içindeki **Static** metotlardır. <br>
**Örn :** Listelerde kullanabildiğiniz bir metot olabilir. <br>

> [!IMPORTANT]
> Bu metotlar `Assets\Scripts\Extentions` klasörünün içindeki `ExtentionMethods` adlı kod dosyasında durmalıdır.

> [!NOTE]
> [ Kısaca Yardımcı Metotlar (Kanal : Tarodev - Dil : İnglizce) ][3]

### Kod Şablonları (Script Templates)

Unity'de oluşturduğumuz kod dosyalarının şablonlarını istediğimiz gibi değiştirebiliriz.

> [!IMPORTANT]
> Şablonu değiştirmek için `D:\Muhammed\Unity\Hub\Editor\2022.3.13f1\Editor\Data\Resources\ScriptTemplates` klasörünün içindeki `81-C# Script-NewBehaviourScript.cs` adlı kod dosyasını değiştirebilirsiniz.

Aşağıdaki örnek benim şablonum
``` 
/*
Name : Muhammed Emin Turkmen
*/

using UnityEngine;

#ROOTNAMESPACEBEGIN#
public class #SCRIPTNAME# : MonoBehaviour
{
   		
}
#ROOTNAMESPACEEND#

```
> [!NOTE]
> [ Kısaca Şablon Değiştirme (Kanal : Tarodev - Dil : İnglizce) ][4]

<!-- Linkler -->
[1]:https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags
[2]:https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
[3]:https://www.youtube.com/watch?v=c1r9Eo3qzlI
[4]:https://www.youtube.com/watch?v=TYRhlOHO_Xk


