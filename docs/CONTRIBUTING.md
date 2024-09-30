# Contribution Guide

本プロジェクトへのコントリビュート方法についてのガイドです。

## 注意事項 かならずお読みください

- [ぴぽや倉庫](https://pipoya.net/sozai/)さんのスプライト画像は、規約の関係上 **無償での再配布のみ** が可能となっており、**有償での再配布が禁止されています**。本ゲームのソースコードおよびアセットを有償で販売する場合は、[ぴぽや倉庫さんの利用規約](https://pipoya.net/sozai/terms-of-use/) 等を参考にしながら、アセットを取り除いた状態での再配布をよろしくお願いします。
- BGM・効果音については、規約の関係上リポジトリに含めない措置をとっています。本リポジトリへコミットする際は、**音源ファイルを含めないようにお願いします**。
- その他、本プロジェクトに対してコントリビュートする際に含まれるアセット（画像や BGM、効果音など）については、それぞれのライセンスに従うものとします。**追加したアセットの利用規約に違反するコミットは削除される場合があることをご了承ください**。

## Issues

次の Issue を受けつけています。

- 使い方など本プロジェクトに関する質問
- エラーや問題の報告
- 新しい機能などの提案

その他の Issue も歓迎しています。

## Pull Request

Pull Request はいつでも歓迎しています。  
次の種類の Pull Request を受け付けています。

以下の内容は Issue を立てずに Pull Request を送ってください。

- 各ドキュメントおよびゲーム内のセリフに関する誤字の修正（変数名や関数名など、プログラムに直結する部分の誤字の修正は含めません）
- 各ドキュメントの修正

以下の内容は Issue を立てて相談してください。

- バグ・不具合の修正
- 新しい機能の追加

## 環境構築

### 1. Unity のインストール

[Unity Hub](https://store.unity.com/download) をダウンロードし、Unity をインストールしてください。
Unity の詳細な使用方法については、[Unity マニュアル](https://docs.unity3d.com/ja/2022.3/Manual/UnityManual.html) などを参照してください。
本プロジェクトは Unity 2022.3.41f1 で開発しています。

### 2. VS Code や Rider などのエディタのインストール

本プロジェクトでは、Unity のスクリプトを編集するために VS Code や Rider などのエディタを使用します。好きなエディタをインストールし、Unity との連携を行ってください。

### 3. リポジトリのクローン

次のコマンドを実行してリポジトリをクローンしてください。

```bash
git clone https://github.com/Undanomi/MWS-Quest.git
```

### 4. 音源ファイルの追加

本プロジェクトでは、BGM や効果音などの音源ファイルを使用しています。音源ファイルは著作権の関係上、リポジトリに含めておりません。**本リポジトリにコントリビュートする際も、音源ファイルは絶対にプッシュしないようにお願いします**。

音源を追加するためには、次の手順を行ってください。

#### 4.1 音源ファイルのダウンロード

次のリンクから音源ファイルをダウンロードしてください。

BGM:

- タイトル・ログイン・シナリオ選択: [Variant](https://booth.pm/ja/items/6114809) から、「MP3(Loop A)」をダウンロード
- ミッション 1 までの BGM: [Arraival](https://zippysound.booth.pm/items/6114852)
- プレイ中: [渡り鳥たちとグランドバザール](https://nostalgic-bgm.booth.pm/items/5773113)
- 逮捕シーン・解説シーン: [ゲームやりすぎ注意報](https://nostalgic-bgm.booth.pm/items/5428749)

効果音: [効果音ラボ](https://soundeffect-lab.info/)

- タイトル画面の決定音: [メニューを開く 3](https://soundeffect-lab.info/sound/button/)
- シナリオ選択の決定音: [キャンセル 7](https://soundeffect-lab.info/sound/button/)
- 決定音: [決定ボタンを押す 48](https://soundeffect-lab.info/sound/button/)
- キャンセル音: [キャンセル 9](https://soundeffect-lab.info/sound/button/)
- ミッション完了音: [決定ボタンを押す 47](https://soundeffect-lab.info/sound/button/)
- 歩行音: [芝生の上を走る](https://soundeffect-lab.info/sound/various/)

#### 4.2 音源ファイルの編集

本プロジェクトのビルド済みソフトウェアでは、音源ファイルのカットや音量調整、速度の調整などを行いバンドルしています。音楽編集ソフトなどを使用して、音源ファイルを編集してください。UN-DANOMI では、[Studio One 6 Prime](https://www.presonus.com/products/Studio-One) を使用して音源ファイルの編集を行っています。編集を行わなくてもゲームの実行自体は可能ですが、**音声ファイルのファイル名は必ず以下の通りにしてください**。

BGM:

- タイトル（[Variant](https://booth.pm/ja/items/6114809)）: Loop A の先頭のフェード部分、および末尾の空白部分をカットし、自然なループ再生が可能となるように編集し、`bgm_title.mp3` として保存
- ログイン・シナリオ選択（[Variant](https://booth.pm/ja/items/6114809)）: タイトルの BGM と同様に編集したのち、再生速度を 1.3 倍に変更し、`bgm_login.mp3` として保存
- ミッション 1 までの BGM（[Arraival](https://zippysound.booth.pm/items/6114852)）:　先頭のフェードイン部分、および末尾の空白部分をカットし、自然なループ再生が可能となるように編集し、`bgm_arrival.mp3` として保存
- プレイ中（[渡り鳥たちとグランドバザール](https://nostalgic-bgm.booth.pm/items/5773113)）: 先頭のフェードイン部分、および末尾の空白部分をカットし、自然なループ再生が可能となるように編集し、`bgm_main.mp3` として保存
- 逮捕シーン・解説シーン（[ゲームやりすぎ注意報](https://nostalgic-bgm.booth.pm/items/5428749)）: 先頭のフェードイン部分、および末尾の空白部分をカットし、自然なループ再生が可能となるように編集し、`bgm_ending.mp3` として保存

効果音:

- タイトル画面の決定音（[メニューを開く 3](https://soundeffect-lab.info/sound/button/)）: 先頭と末尾の空白部分をカットし、`se_title.mp3` として保存
- シナリオ選択の決定音（[キャンセル 7](https://soundeffect-lab.info/sound/button/)）: 先頭と末尾の空白部分をカットし、`se_select.mp3` として保存
- 決定音（[決定ボタンを押す 48](https://soundeffect-lab.info/sound/button/)）: 先頭と末尾の空白部分をカットし、`se_decision.mp3` として保存
- キャンセル音（[キャンセル 9](https://soundeffect-lab.info/sound/button/)）: 先頭と末尾の空白部分をカットし、`se_cancel.mp3` として保存
- ミッション完了音（[決定ボタンを押す 47](https://soundeffect-lab.info/sound/button/)）: そのまま、`se_correct.mp3` として保存
- 歩行音（[芝生の上を走る](https://soundeffect-lab.info/sound/various/)）: 先頭と末尾の空白部分をカットし、2 倍速に変更し、さらに最大音量が`0dB` 程度になるように調整し、`se_footstep.mp3` として保存

#### 4.3 音源ファイルの配置

音源ファイルを以下のディレクトリに直接配置してください（ない場合は作成してください）。

- BGM（上記で`bgm_`から始まるファイル名にリネームしたもの）: `プロジェクトディレクトリ/Assets/Resources/Sounds/BGM/`
- 効果音（上記で`se_`から始まるファイル名にリネームしたもの）: `プロジェクトディレクトリ/Assets/Resources/Sounds/SE/`

### 5. プロジェクトの起動

Unity Hub を開き、`Add` をクリックしてクローンしたリポジトリを選択してください。プロジェクトが開き、編集や実行、ビルドが可能になります。
