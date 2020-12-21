# DataMiningSpotifyTop


### Spotify song datasets
1. https://www.kaggle.com/leonardopena/top-spotify-songs-from-20102019-by-year
2. https://www.kaggle.com/yamaerenay/spotify-dataset-19212020-160k-tracks
3. https://www.kaggle.com/theoverman/the-spotify-hit-predictor-dataset
4. https://www.kaggle.com/nadintamer/top-tracks-of-2017
5. https://www.kaggle.com/nadintamer/top-spotify-tracks-of-2018
6. https://www.kaggle.com/leonardopena/top50spotify2019

### Target dataset
https://www.kaggle.com/leonardopena/top-spotify-songs-from-20102019-by-year

### Задание
0. Разбиться на команды по 2-3 человека (опционально).
1. Придумать себе тему.
2. Раздобыть датасет.
3. Построить математическую модель.
4. Самостоятельно закодировать соответствующие алгоритмы.
5. Проанализировать точность модели.
6. Сделать выводы.

### Постановка задачи

Большинство песен относятся к жанрам, в названии которых есть "pop". По этому признаку делить песни бесполезно. Хорошо бы найти какие-то закономерности в песнях в топе. При каких характеристиках песни вероятнее всего попасть в топ. Выпустил песню, Spotify проанализировал ее, определил значения всех параметров (характеристик, фич), а далее по ним попробовать примерно спрогнозировать успешность композиции.

### Spotify Web API
1. [Audio Features API](https://developer.spotify.com/documentation/web-api/reference/tracks/get-audio-features/)

### Алгоритмы k-means
1. [Метод k-средних](https://wiki.loginom.ru/articles/k-means.html)
2. [Алгоритмы кластеризации Microsoft](https://docs.microsoft.com/ru-ru/analysis-services/data-mining/microsoft-clustering-algorithm?view=asallproducts-allversions)
3. [Обзор алгоритмов кластеризации данных](https://habr.com/ru/post/101338/)
4. [Алгоритмы k-means и c-means](https://habr.com/ru/post/67078/)
5. [k-means++](https://ru.wikipedia.org/wiki/K-means%2B%2B)
6. [k-means](https://ru.qaz.wiki/wiki/K-means_clustering)
7. [Анализ модели: среднее внутрикластерное расстояние, среднее межкластерное расстояние, коэффициент силуэта](https://ru.coursera.org/lecture/unsupervised-learning/otsienka-kachiestva-i-riekomiendatsii-po-rieshieniiu-zadachi-klastierizatsii-FKgPk)

### Зависимости
1. [CSV Serializer](https://github.com/DenisBelobrotski/CsvSerializer)
2. [Plotting Tool](https://github.com/swharden/ScottPlot)

### Заметки
1. Внутрикластерное расстояние для каждой модели.
1. Среднее внутрикластерное расстояние по всем моделям.
1. Среднее межкластерное расстояние по всем моделям.
1. Коэффициент силуэта. Может быть [-1, 1]. Больше - лучше. В хорошем случае > 0. Считается для каждой точки. Можно считать среднее значение по модели, а также среднее значение по каждой модели. Аналогично внутрикластерному расстоянию.
1. Best clusters count: 3, 4, 5.
1. Убрать шумы.
1. Не учитывается instrumentalness.
1. Из-за перехода к процентам (целым числам) в датасете потеряна точность координат.
1. Неточности могут быть еще и из-за анализатора песен Spotify.
