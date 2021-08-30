<template>
  <v-row>
    <v-col
      cols="12"
      md="9"
    >
      <forum-list
        v-for="category of categories"
        :key="category.uid"
        :category="category"
        :forums="forums[category.uid]"
        :loading="loading"
        class="mb-4"
      >
      </forum-list>
    </v-col>

    <v-col
      cols="12"
      md="3"
    >
      <app-forum-statistics></app-forum-statistics>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue';
import { onMounted, reactive, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import { useCategories } from '@/composable/form';
import { Category, Forum, SingularDictionary } from '@/services/model';
import { useHttp } from '@/services/http';
import ForumList from '@/components/forum/ForumList.vue';
import { useBreadcrumbs } from '@/composable/breadcrumbs';

export default Vue.extend({
  name: 'Home',
  components: { ForumList, AppForumStatistics },
  setup() {
    useTitle('Home');
    useBreadcrumbs([{ text: 'Home', name: 'Home' }]);

    const { data: allCategories } = useCategories();
    const loading = ref(true);
    const categories: Category[] = allCategories.value.slice(0, 3);
    const forums = reactive<SingularDictionary<Forum[]>>(
      categories.reduce((obj, cat) => {
        obj[cat.uid] = [];
        return obj;
      }, {}),
    );

    const http = useHttp();
    onMounted(async () => {
      try {
        const res = await http.get<Forum[]>('tracking/active-forums', {
          params: {
            categories: categories.map((c) => c.slug),
          },
        });
        const forumByCategories: SingularDictionary<Forum[]> = { ...forums };
        res.forEach((forum) => {
          const list = forumByCategories[forum.category.uid] ?? [];
          list.push(forum);
          forumByCategories[forum.category.uid] = list;
        });
        Object.assign(forums, forumByCategories);
      } finally {
        loading.value = false;
      }
    });

    return {
      categories,
      forums,
      loading,
    };
  },
});
</script>
