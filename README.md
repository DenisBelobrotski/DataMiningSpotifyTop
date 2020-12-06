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

### Зависимости
1. [CSV Serializer](https://github.com/DenisBelobrotski/CsvSerializer)
2. [Plotting Tool](https://github.com/swharden/ScottPlot)

### To Do List
1. Выбирать на первом шаге не рандомные точки, самые отдаленные друг от друга.
2. Попробовать c-means.
