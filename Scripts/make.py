import pandas as pd
import random
import datetime

dt = datetime.datetime.now()

# CSVファイルリスト
csv_files = ['alice.csv', 'bob.csv', 'charlie.csv', 'dave.csv', 'ellen.csv', 'frank.csv', 'core.csv']
files = []
for file in csv_files:
    with open(file, "r") as f:
        files.append(f.read().splitlines())

merged_data = []
blue = '00FF00'
red = 'FF66FF'
yellow = 'FFC000'

while len(files) > 0:
    select = random.randint(0, len(files) - 1)
    item = files[select].pop(0).split(',')
    dt += datetime.timedelta(seconds=random.randint(0, 100))
    if(item[1] == ''):
        merged_data.append(f'{dt.strftime("%H:%M:%S")} <color=#{blue}>{item[0]}</color>は<color=#{yellow}>{item[2]}</color>')
    else:
        merged_data.append(f'{dt.strftime("%H:%M:%S")} <color=#{blue}>{item[0]}</color>は<color=#{red}>{item[1]}</color>で<color=#{yellow}>{item[2]}</color>')
    if len(files[select]) == 0:
        files.pop(select)

with open("log.txt", "w") as f:
    f.write('\n'.join(merged_data))