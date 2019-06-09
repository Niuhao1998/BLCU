import paddle
import paddle.fluid as fluid
from functools import partial
import numpy as np
import sys

def Dynamic_rnn(data, input_dim, class_dim, emb_dim, lstm_size):
    emb=fluid.layers.embedding(
        input=data,size=[input_dim,emb_dim],is_sparse=True
    )
