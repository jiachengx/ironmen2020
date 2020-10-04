# coding: utf-8
from playwright import sync_playwright
with sync_playwright() as p:
    for browser_type in [p.chromium, p.firefox, p.webkit]:
        browser = browser_type.launch()
        page = browser.newPage()
        page.goto('https://ithelp.ithome.com.tw/2020-12th-ironman?utm_source=iThelp&utm_medium=nav&utm_campaign=ironman')
        page.screenshot(path=f'ironmenSample-{browser_type.name}.png')
        browser.close()
