# Unclazz.Commons.Json

`Unclazz.Commons.Json`はJSON形式のデータを.NETのアプリケーションから参照・更新するためのライブラリです。v1．0ではJSONのパース、組み立て、そしてフォーマットをサポートしています。アセンブリは[NuGet Galleryで公開](https://www.nuget.org/packages/Unclazz.Commons.Json/)されているので、NuGetを通じて取得することができます。

## コンポーネント

### 名前空間

アセンブリ名と同じ`Unclazz.Commons.Json`がメインのコンポーネントが属する名前空間です。JSONの読み込み、組み立て、フォーマットのためAPIが詰まっています。
一方`Unclazz.Commons.Json.Parser`は前述のAPIが内部的に利用しているパーサが属する名前空間です。一般用途であればこちらを直接利用する必要はないはずです。

### インターフェース

`IJsonObject`は文字通りJSON形式データの中のオブジェクト──`Object`、`Array`、`String`、`Number`、`Boolean`、`Null`を表わすものです。インスタンスが`Object`型を表わす場合、`IJsonObject#GetProperty(...)`や`IJsonObject#HasProperty(string)`メソッドを通じてそのプロパティにアクセスできます。

`IJsonFormatOptions`はJSON形式データをフォーマットする際のオプションを保持するインターフェースです。このインターフェースは　`IJsonObject#Format(...)`とともに利用します。

### クラス

`JsonObject`クラスは各種の静的ファクトリ・メソッド公開するユーティリティであるとともに、`IJsonObject`インターフェースを実装した非公開の具象クラスにテンプレート・メソッドを提供する抽象クラスでもあります。

`JsonObject.FromString(string)`・`JsonObject.FromFile(string)`・`JsonObject.FromStream(Stream)`の3つの静的メソッドはそれぞれ異なる種類のデータソースからJSON形式データを読み込むためのものです。`JsonObject.Builder()`はJSON形式データを組み立てるためのビルダーのインスタンスを生成するものです。

`JsonFormatOptions`クラスは`IJsonFormatOptions`のデフォルト実装であり、その組立には`JsonFormatOptions.Builder()`が返すビルダーを使用します。

## 使用例

### 1. JSONを読み込む

```cs
// JsonObject.FromXxxx methods read JSON from String, File and Stream.
IJsonObject j1 = JsonObject.FromString("{foo: 123, bar: true, baz: \"hello\"}");
// IJsonObject#GetProperty() accesses JSON object's property.
// NOTE: GetPropery() and XxxxValue() methods have another version
// that can be specified fallback value.
Console.WriteLine("j1 = {0}", j1);
Console.WriteLine("foo = {0}", j1.GetProperty("foo").AsNumber());
Console.WriteLine("bar = {0}", j1.GetProperty("bar").AsBoolean());
Console.WriteLine("baz = {0}", j1.GetProperty("baz").AsString());
Console.WriteLine("baa exists? = {0}", j1.HasProperty("baa"));
```
### 2. JSONを組み立てる

```cs
// JsonObject.Builder() returns new builder instance.
IJsonObject j2 = JsonObject
	.Builder()
	.Append("foo", "hello")
	.Append("bar", "hello", "bonjour", "こんにちは")
	.Append("baz", (b) => {
		// Lambda's argument is new builder
		// instance for nested object.
		b.Append("bazProp1", 1);
		b.Append("bazProp2", 2);
	})
	.Build();
// The builder can be initialized with existed JSON.
IJsonObject j3 = JsonObject
	.Builder(j2).Append("baa", 123).Build();
Console.WriteLine("j2 = {0}", j2);
Console.WriteLine("j3 = {0}", j3);
```

### 3. JSONをフォーマットする

```cs
IJsonFormatOptions opts = JsonFormatOptions
	.Builder()
	.Indent(true).SoftTabs(true).TabWidth(2)
	.Build();
Console.WriteLine("j3' = {0}", j3.Format(opts));
```
