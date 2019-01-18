# -*- coding: utf-8 -*-
"""
Created on Sat Dec  1 13:53:15 2018

@author: John and Julia
"""

import requests
import bs4
import pandas as pd
import numpy as np
import datetime
import os
import time

def getSoup(baseUrl):
    url = baseUrl
    
    while (True):
        page = requests.get(url)
    
        if (page.status_code == 200):
            return bs4.BeautifulSoup(page.content, 'lxml')
        
        time.sleep(5)
        print('slept')

def getTable(soup):
    
    table = soup.find(name='div', attrs={'id':'cpm'}).find(name='table')
    
    if (table is not None):
        return table.find_all('tr')

    return None

def getIndexesToSplit(table):
    indexes = []
    for tr in table:
        if (tr.find('th') is not None):
            indexes.append(table.index(tr))
            
    return indexes

def splitTables(table, indexes):
    tables = []
    for i in range(0, len(indexes)):
        if (i < len(indexes) - 1):
            tables.append(table[indexes[i]:indexes[i+1]])
        else:
            tables.append(table[indexes[i]:])
            
    return tables

def getData(table):
    df = pd.DataFrame([[td.text for td in row.findAll('td')] for row in table], columns=['A', 'B', 'C', 'Home Team', 'Score', 'Away Team', 'D'])
    df.drop(['A','B','C','D'], axis=1, inplace=True)
    
    return df

def transformData(df, date, leagueName):
    df.loc[df['Score'] == 'v', 'Score'] = '-'
    df['Home Goals'] = df['Score'].str.split('-').str.get(0).str.strip()
    df['Away Goals'] = df['Score'].str.split('-').str.get(1).str.strip()

    df.drop(['Score'], axis=1, inplace=True)
    
    df['Date'] = date
      
    if (leagueName == 'Premiership' or leagueName == 'English Div 1 (old)'):
        df['Tier'] = '1'
    elif (leagueName == 'Football League Div 1' or leagueName == 'English Div 2 (old)'):
        df['Tier'] = '2'
    elif (leagueName == 'Football League Div 2' or leagueName == 'English Div 3 (old)'):
        df['Tier'] = '3'
    elif (leagueName == 'English Div 3 (north)'):
        df['Tier'] = '3N'
    elif (leagueName == 'English Div 3 (south)'):
        df['Tier'] = '3S'
    elif (leagueName == 'Football League Div 3' or leagueName == 'English Division 4'):
        df['Tier'] = '4'
    else:
        df['Tier'] = np.NaN
          
    df.drop(df[pd.isnull(df.Tier)].index, inplace=True)
    df = df.reindex(columns=['Date', 'Home Team', 'Away Team', 'Home Goals', 'Away Goals', 'Tier'])

    return df
        
def main():
    filePath = os.path.join("C:\\","Users","John and Julia","Desktop")
    date = datetime.date(1993, 5, 31)
    endDate = datetime.date(1888, 7, 31)
    
    output = pd.DataFrame(columns=['Date', 'Home Team', 'Away Team', 'Home Goals', 'Away Goals', 'Tier'])
    while(date > endDate):       
        url = "https://www.soccerbase.com/matches/results.sd?date=" + str(date)
        soup = getSoup(url)
        
        table = getTable(soup)
        if (table is not None):
            indexes = getIndexesToSplit(table)
            tables = splitTables(table, indexes)
            
            for table in tables:
                leagueName = table.pop(0).find('h2').text
                
                for row in table[::-1]:
                    if (row.find('td', attrs={'class':'matchInfo'})):
                        table.remove(row)
                
                df = getData(table)
                matches = transformData(df, date, leagueName)
                
                output = output.append(matches)
        
        output.to_csv(os.path.join(os.path.join(filePath, 'scrapeResults.csv')), index=None, sep='|', encoding='iso-8859–1')

        print(str(date) + " - " + str(len(output)))
        
        date = date - datetime.timedelta(days = 1)
        if (date.month == 7 or date.month == 6):
            date = datetime.date(date.year, 5, 31)
    
main()