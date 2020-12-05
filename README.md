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

### Постановка задачи

Большинство песен относятся к жанрам, в названии которых есть "pop". По этому признаку делить песни бесполезно. Хорошо бы найти какие-то закономерности в песнях в топе. Чтобы по характеристикам было понятно, какие песни попадают в топ. То можно понять по характеристикам самого большого кластера.

При каких характеристиках вероятнее всего попасть в топ.

Выпустил песню, Spotify проанализировал ее, определил значения всех параметров, а далее по ним можно примерно спрогнозировать успешность композиции.

### Spotify Web API
https://developer.spotify.com/documentation/web-api/reference/tracks/get-audio-features/

### Алгоритмы k-means
1. https://wiki.loginom.ru/articles/k-means.html
2. https://docs.microsoft.com/ru-ru/analysis-services/data-mining/microsoft-clustering-algorithm?view=asallproducts-allversions
3. https://habr.com/ru/post/101338/
4. https://habr.com/ru/post/67078/

### Зависимости
https://github.com/DenisBelobrotski/CsvSerializer
https://github.com/swharden/ScottPlot

### To Do List
1. Выбирать на первом шаге не рандомные точки, самые отдаленные друг от друга.
2. Попробовать c-means.
