<template>
  <v-row class="py-2">
    <v-col cols="2" xl="1" class="d-flex flex-column align-center justify-center">
      <v-avatar>
        <v-img
          :src="post.user.avatar || require('@/assets/anon.png')"
          lazy-src="@/assets/anon_thumbnail.png"
          :alt="post.user.username"
        ></v-img>
      </v-avatar>
      <div style="max-width: 100%">
        <div class="text-truncate caption font-weight-medium">
          {{ post.user.username }}
        </div>
      </div>
    </v-col>
    <v-col cols="10" xl="11" class="pt-0 d-flex flex-column">
      <div class="d-flex">
        <div class="d-flex align-center mr-2" v-if="!post.postsCount">
          <v-icon class="material-icons-outlined mr-1" x-small>schedule</v-icon>
          <span>{{ formatDateTime(post.createdAt) }}</span>
        </div>
        <div class="d-flex align-center" v-else>
          <v-icon small class="mr-1">star_outline</v-icon>
          <span>Origin Post</span>
        </div>
      </div>
      <slot name="title"></slot>
      <div class="body-1" v-html="post.content" style="max-width: 100%; overflow: hidden"></div>
      <v-spacer></v-spacer>

      <div class="mt-2 d-inline">
        <v-btn icon small @click="vote(1)" :color="post.voted > 0 ? 'primary' : ''">
          <v-icon>arrow_drop_up</v-icon>
        </v-btn>
        <span>{{ post.vote }}</span>
        <v-btn icon small @click="vote(-1)" :color="post.voted < 0 ? 'primary' : ''">
          <v-icon>arrow_drop_down</v-icon>
        </v-btn>
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import { Post } from '@/services/model';
import NumberCounter from '@/components/NumberCounter.vue';
import { useHttp } from '@/services/http';
import { isAuthorized } from '@/services/auth';
import { useMessage } from '@/composable/message';
import { useRouter } from '@/composable/compat';

export default defineComponent({
  name: 'PostRow',
  components: { NumberCounter },
  props: {
    post: {
      required: true,
      type: Object as PropType<Post>,
    },
  },
  setup(props) {
    const http = useHttp();
    const { confirm } = useMessage();
    const router = useRouter();

    async function vote(vote: number) {
      if (!isAuthorized('User')) {
        const ok = await confirm({
          title: 'Vote',
          text: 'You must login to perform this action',
        });
        if (ok) await router.push({ name: 'Login' });
        return;
      }
      const { post } = props;
      const voted = vote < 0 ? downVote(post) : upVote(post);
      if (post.voted === 0) post.vote += voted;
      else if (voted === 0) post.vote -= post.voted;
      else post.vote += (voted * 2);
      post.voted = voted;
    }

    function upVote(post: Post): number {
      const vote = post.voted > 0 ? 0 : 1;
      http.post(`posts/${post.uid}/votes/${vote}`);
      return vote;
    }

    function downVote(post: Post): number {
      const vote = post.voted < 0 ? 0 : -1;
      http.post(`posts/${post.uid}/votes/${vote}`);
      return vote;
    }

    return {
      vote,
      formatDateTime,
    };
  },
});
</script>
