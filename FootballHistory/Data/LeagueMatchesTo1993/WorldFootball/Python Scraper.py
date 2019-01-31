import requests
import bs4
import pandas as pd
import numpy as np
import os

def getSoup(baseUrl):
    url = baseUrl
    page = requests.get(url)
    return bs4.BeautifulSoup(page.content, 'lxml')

def getDataTables(soup):
    return soup.find(name='div', attrs={'class':'content'}).find_all(name='table', attrs={'class':'standard_tabelle'})
    
def buildDataFrame(table):
    df = pd.DataFrame([[td.text for td in row.findAll('td')] for row in table.findAll('tr')], columns=['Date', 'A', 'Home Team', 'B', 'Away Team', 'Score', 'C', 'D'])
    df.drop(['A','B','C','D'], axis=1, inplace=True)
    
    return df
    
def transformData(matches):
    df = matches.apply(lambda x: x.str.strip()).replace('', np.nan)
    df.fillna(method='ffill', inplace=True)
    
    df['Score'] = df['Score'].str.split('(').str.get(0)
    df['Home Goals'] = df['Score'].str.split(':').str.get(0)
    df['Away Goals'] = df['Score'].str.split(':').str.get(1)
    df.drop(['Score'], axis=1, inplace=True)
    
    return df

def getLink(soup, previousSeason):
    table = soup.find(name='div', attrs={'class':'content'}).find(name='table', attrs={'class':'auswahlbox with-border'}).find_all('td')
    
    if (previousSeason):
        return table[0].find('a')
    
    return table[4].find('a')
    
def getTierResults(tier, baseUrl, fileName):
    filePath = os.path.join("C:\\","Data")
    url = baseUrl
    output = pd.DataFrame(columns=['Date', 'Home Team', 'Away Team', 'Home Goals', 'Away Goals'])

    allSeasonsScraped = False
    while (not allSeasonsScraped):
        print(url)
        
        numRounds = 1
        allRoundsScraped = False
        while (not allRoundsScraped):
            print(numRounds)
            soup = getSoup(url)
            tables = getDataTables(soup)
    
            df = buildDataFrame(tables[0])
            matches = transformData(df)
            
            output = output.append(matches)
            
            link = getLink(soup, False)
            if (link is not None):
                href = link.get('href')
                round = href.split("/")[3]
                
                if (round == "0"):
                    allRoundsScraped = True
                else:
                    url = "https://www.worldfootball.net" + href
                    numRounds += 1
            else:
                allRoundsScraped = True
        
        output.to_csv(os.path.join(os.path.join(filePath, fileName)), index=None, sep='|', encoding='iso-8859–1')

        link = getLink(soup, True)
        if (link is not None):
            url = "https://www.worldfootball.net" + link.get('href')
        else:
            allSeasonsScraped = True
    
    output['Tier'] = tier

def Main():
    getTierResults(1, "https://www.worldfootball.net/schedule/eng-premier-league-1992-1993-spieltag/", "PremierLeague.csv")
    getTierResults(2, "https://www.worldfootball.net/schedule/eng-championship-1992-1993-spieltag/", "Championship.csv")
    
Main()
