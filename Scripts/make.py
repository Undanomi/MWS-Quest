import pandas as pd
import random
import datetime

# CSVファイルリスト
time_table = {
    datetime.datetime(2024, 9, 27, 9, 0, 0): ['data/bob.csv'],
    datetime.datetime(2024, 9, 27, 11, 55, 0): ['data/alice.csv', 'data/dave.csv'],
    datetime.datetime(2024, 9, 27, 16, 35, 0): ['data/charlie.csv', 'data/frank.csv'],
    datetime.datetime(2024, 9, 27, 19, 0, 0): ['data/core.csv'],
    datetime.datetime(2024, 9, 27, 21, 20, 0): ['data/ellen.csv'],
}

merged_data = []
blue = '00FF00'
red = 'FF66FF'
yellow = 'FFC000'

for key, csv_files in time_table.items():
    dt = key
    files = []
    for file in csv_files:
        with open(file, "r") as f:
            files.append(f.read().splitlines())
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