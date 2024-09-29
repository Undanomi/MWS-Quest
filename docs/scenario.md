# シナリオ・設定

## 元となる攻撃シナリオ

1. 悪性マクロ付きのExcelファイルをWebブラウザを使用してダウンロード
2. Excelファイルを開く
3. 保護ビューを解除
4. 悪性マクロが機密情報 (C:\Users\alice\Document\*) にアクセスし、機密情報を持ち出し

## 世界観

- プレイヤーが住む町，アルファ村(仮)が舞台
- アルファ村は様々な農作物を他の村に売っている村である
- プレイヤーはアルファ村の衛兵である
- アルファ村の衛兵は、村内で使用された呪文の痕跡を確認できる機械を使用できる

### 用語の言い換え

| 現実の用語 | ゲーム内の用語 |
|---|---|
| コンピュータ | 村 |
| ディレクトリ | 村の中の1エリア or 家 or 部屋 |
| ファイル | 本 or 書物 |
| コマンド | 呪文 |
| rootユーザ | 村長 |
| 一般ユーザ | 村人 |
| Excelファイル | 帳簿管理の魔法の書 |
| 保護ビュー | 封印 |
| `C:\Users\alice\Document\*` | 交易責任者 (アリス) 宅の書庫 |
| EDRログ | 呪文の痕跡 |
| EDR | 呪文の痕跡を確認できる機械 |

## シナリオ

### 概要

- アルファ村では、近隣の村との交易に関する機密書類(交易記録)が流出していることが判明した
- 最近使用された呪文の痕跡を調べて、流出の原因を特定しよう

### 事件のあらまし

1. アルファ村のアリスが、帳簿管理の魔法の書をブラボ―村のイブから貰う
2. アリスは帳簿管理の魔法の書を読むために開く
3. 帳簿管理の魔法の書の一部には封印がかかっていて読めないため、アリスは封印を解除
4. アリスは気づかなかったが、帳簿管理の魔法の書に仕込まれていた呪文が、アリス宅の書庫にある書類をイブに送る

## ゲームの目的・クリア条件

- 本ゲームの舞台となるアルファ村で起こった事件の原因を特定し，事件を解決する
1. 取引記録が流出した原因を特定する
2. 帳簿管理の魔法の書が持ち込まれたルートを特定する
3. 以上2点を村長へ報告する