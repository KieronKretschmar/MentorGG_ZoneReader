de_inferno.xml von Ende September 2017:
Targets: Wie unten, mit merge_targets() mehrere Targets verbunden (e.g. Stufen im Connector)
Kategorien:
- nach ['PlayerPosX', 'PlayerPosY', 'PlayerPosZ', 'PlayerViewX', 'PlayerViewY'] clustern
- mit show() oder show(target_id = ...) einzelne Kategorien aussuchen
- mit compare_resized(id) testen, ob dies nur eine Kategorie ist, und ob man noch resizen (= Toleranzen hinzuf�gen) kann, um auch Fehlversuche zu registrieren
	- Wenn es mehrere sind, muss feiner geclustert werden
	- Wenn m�glich (In den meisten F�llen) mit resize() Toleranzen Settings.CATEGORY_MARGINS['default'] hinzuf�gen
- mit add() hinzuf�gen


de_dust2.xml von Mitte September '17 wurde folgenderma�en erstellt:
	Zuerst wurde jedes Target mit den folgenden Schritten erstellt.
		1. Mit generate(eps, min_samples) mehrere cluster nach (x,y,z) Landeort der Smokes gebildet.
		2. Ein Cluster ausw�hlen und einen purpose dieses Targets definieren (z.B. 'Block LoS CT Spawn to Long at Cross')
		3. So lange das eps ver�ndern, bis das Target so gro� ist, dass es genau alle Smokes 
		   die diesem Purpose irgendwie dienlich sind, zu beinhalten
		4. Mit add(id, purpose) das Target hinzuf�gen.

	Dann die Kategorien mit folgenden Schritten hinzuf�gen:
		1. Mit generate(eps, min_samples) mehrere Cluster gebildet
			a) F�r Kategorien mit ID < 9 gilt settings.CLUSTER_COLS['category'] = ['PlayerPosX', 'PlayerPosY', 'PlayerPosZ', 'PlayerViewX', 'PlayerViewY', 'GrenadePosX', 'GrenadePosY', 'GrenadePosZ']
			b) F�r Kategorien mit ID > 9 gilt settings.CLUSTER_COLS['category'] = ['PlayerPosX', 'PlayerPosY', 'PlayerPosZ', 'PlayerViewX', 'PlayerViewY']
			Ich bin gespannt, was besser funktioniert. Bei a) werden solche Smokes geclustert, die nah beieinander landen (Erfolge),
			Bei b) solche, die bei Abwurf �hnlich waren (aber potentiell an verschiedenen Orten landen).
			Thesen: b) hat eine leicht h�here Failrate. a) �bersieht Fehlversuche. Urspr�ngliche Landezonen von b) sind gr��er, da auch Fails dabei sind.
		2. Ein Cluster ausw�hlen und einen Namen dieser Kategorie definieren (z.B. 'A Long Doors to Cross')
		3. Das eps so klein wie m�glich, aber so gro� wie n�tig w�hlen, sodass alle Versuche dieser Smoke zur Kategorie z�hlen. �berpr�fen durch detailliertes show().
		4. Mit add(id, name) die Kategorie hinzuf�gen. Kommen mehrere Targets in betracht, auch die passende target_id angeben.
		5. Die Kategorie umfasst aktuell alle Abw�rfe von erfolgreichen Versuchen, nicht jedoch von fehlgeschlagenen. 
			Die Abwurfzone muss jedoch evtl. um einen Faktor alpha vergr��ert werden, sodass auch Fehlversuche aufgenommen werden, welche sonst nicht zu dieser Kategorie z�hlen w�rden.
			Mit compare_resizes(alphas) k�nnen verschiedene Vergr��erungen getestet werden. 
^			Ein soll ein alpha zwischen 1 und 1.5 gefunden werden, dass klein genug ist, sodass keine/kaum smokes mit anderen Intentionen erfasst werden, wohl aber zus�tzlich Fehlgeschlagene
			Mit category.resize(alpha) kann die Abwurfzone dann vergr��ert werden.