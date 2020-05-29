Data was obtained by scraping from two sources:


1. https://www.worldfootball.net

Using `Python Scraper.py`, all results in the top tier of English Football up to 1992-1993 and all results from 1946-1947 to 1992-1993 in the second tier were scraped.
No results were available before 1946 for the second tier and no results were available at all for the third and fourth tiers.

2. https://www.soccerbase.com

Using `Python Scraper 2.py`, all results in all four tiers of English Football were scraped from inception of each tier to the 1992-1993 seasons. The scraper sometimes got kicked, so there are lots of raw scraped files covering the time-frame which have been consolidated into a single `1888-1992.csv` file.
