import requests as r

cmd = input()
url = 'http://sosik.pythonanywhere.com/users/sanya/command'
data = {'cmd': f'{cmd}'}

r.post(url, data=data)
