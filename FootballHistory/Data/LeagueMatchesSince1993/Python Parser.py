import csv
import glob

def parser():
    files = glob.glob('C:\\Data\\*.csv')
    target = open('C:\\Data\\result.csv', 'w')
    
    for file in files:
        with open(file) as csvfile:
            readCSV = csv.reader(csvfile, delimiter=',')
    
            for row in readCSV:   
                div = row[0]
                date = row[1]
                home = CleanseTeams(row[2])
                away = CleanseTeams(row[3])
                gf = row[4]
                ga = row[5]

                cleansedDate = CleanseDate(date)
                    
                if div != '' and div != 'Div':
                    div = CleanseCompetition(div, cleansedDate)
                    target.write(div + "," + cleansedDate + "," + home + "," + away + "," + gf + "," + ga + "\n")
                   
    target.close()

def CleanseTeams(team):
    if team == "Nott'm Forest":
        return "Nottingham Forest"
    elif team == "Accrington":
        return "Accrington Stanley"
    elif team == "Aldershot":
        return "Aldershot Town"
    elif team == "Birmingham":
        return "Birmingham City"
    elif team == "Blackburn":
        return "Blackburn Rovers"
    elif team == "Bolton":
        return "Bolton Wanderers"
    elif team == "Boston":
        return "Boston United"
    elif team == "Bournemouth":
        return "AFC Bournemouth"
    elif team == "Bradford":
        return "Bradford City"
    elif team == "Brighton":
        return "Brighton and Hove Albion"
    elif team == "Bristol Rvs":
        return "Bristol Rovers"
    elif team == "Burton":
        return "Burton Albion"
    elif team == "Cambridge":
        return "Cambridge United"
    elif team == "Cardiff":
        return "Cardiff City"
    elif team == "Carlisle":
        return "Carlisle United"
    elif team == "Charlton":
        return "Charlton Athletic"
    elif team == "Cheltenham":
        return "Cheltenham Town"
    elif team == "Chester":
        return "Chester City"
    elif team == "Colchester":
        return "Colchester United"
    elif team == "Coventry":
        return "Coventry"
    elif team == "Crewe":
        return "Crewe Alexander"
    elif team == "Dag and Red":
        return "Dagenham and Redbridge"
    elif team == "Derby":
        return "Derby County"
    elif team == "Doncaster":
        return "Doncaster Rovers"
    elif team == "Exeter":
        return "Exeter City"
    elif team == "Forest Green":
        return "Forest Green Rovers"
    elif team == "Grimsby":
        return "Grimsby Town"
    elif team == "Halifax":
        return "Halifax Town"
    elif team == "Hartlepool":
        return "Hartlepool United"
    elif team == "Hereford":
        return "Hereford United"
    elif team == "Huddersfield":
        return "Huddersfield Town"
    elif team == "Ipswich":
        return "Ipswich Town"
    elif team == "Kidderminster":
        return "Kidderminster Harriers"
    elif team == "Leeds":
        return "Leeds United"
    elif team == "Leicester":
        return "Leicester City"
    elif team == "Lincoln":
        return "Lincoln City"
    elif team == "Luton":
        return "Luton Town"
    elif team == "Macclesfield":
        return "Macclesfield Town"
    elif team == "Man City":
        return "Manchester City"
    elif team == "Man United":
        return "Manchester United"
    elif team == "Mansfield":
        return "Mansfield Town"
    elif team == "Middlesboro":
        return "Middlesborough"
    elif team == "Milton Keynes D":
        return "Milton Keynes Dons"
    elif team == "Newcastle":
        return "Newcastle United"
    elif team == "Northampton":
        return "Northampton Town"
    elif team == "Norwich":
        return "Norwich City"
    elif team == "Oldham":
        return "Oldham Athletic"
    elif team == "Oxford":
        return "Oxford United"
    elif team == "Peterboro":
        return "Peterborough United"
    elif team == "Plymouth":
        return "Plymouth Argyle"
    elif team == "Preston":
        return "Preston North End"
    elif team == "Rotherham":
        return "Rotherham United"
    elif team == "Rushden & D":
        return "Rushden and Diamonds"
    elif team == "Scunthorpe":
        return "Scunthorpe United"
    elif team == "Sheffield Weds":
        return "Sheffield Wednesday"
    elif team == "Shrewsbury":
        return "Shrewsbury Town"
    elif team == "Southend":
        return "Southend United"
    elif team == "Stockport":
        return "Stockport County"
    elif team == "Stoke":
        return "Stoke City"
    elif team == "Swansea":
        return "Swansea City"
    elif team == "Swindon":
        return "Swindon Town"
    elif team == "Torquay":
        return "Torquay United"
    elif team == "Tottenham":
        return "Tottenham Hotspur"
    elif team == "Tranmere":
        return "Tranmere Rovers"
    elif team == "West Brom":
        return "West Bromwich Albion"
    elif team == "West Ham":
        return "West Ham United"
    elif team == "Wigan":
        return "Wigan Athletic"
    elif team == "Wolves":
        return "Wolverhampton Wanderers"
    elif team == "Wycombe":
        return "Wycombe Wanderers"
    elif team == "Yeovil":
        return "Yeovil Town"
    elif team == "York":
        return "York City"
    
    return team

def CleanseDate(parsedDate):
    cleansedDate = parsedDate
    if len(parsedDate) == 8:
        if parsedDate[6] == '9':
            cleansedDate = parsedDate[0:6] + '19' + parsedDate[6:]
        else:
            cleansedDate = parsedDate[0:6] + '20' + parsedDate[6:]

    return cleansedDate[6:] + '-' + cleansedDate[3:5] + '-' + cleansedDate[0:2]

def CleanseCompetition(division, date):
    div = ''
    year = int(date[0:4])
    month = int(date[5:7])
    
    if division == 'E0':
        div = 'Premier League'
    elif division == 'E1' and year < 2004 or (year == 2004 and month <= 6):
        div = 'Championship'
    elif division == 'E1' and year > 2004 or (year == 2004 and month >= 7):
        div = 'Division 1'
    elif division == 'E2' and year < 2004 or (year == 2004 and month <= 6):
        div = 'League 1'
    elif division == 'E2' and year > 2004 or (year == 2004 and month >= 7):
        div = 'Division 2'
    elif division == 'E3' and year < 2004 or (year == 2004 and month <= 6):
        div = 'League 2'
    elif division == 'E3' and year > 2004 or (year == 2004 and month >= 7):
        div = 'Division 3'

    return div
    
parser()
