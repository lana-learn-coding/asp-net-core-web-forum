<template>
  <v-card flat>
    <v-card-title class="justify-center">{{
        user.uid ? user.username : 'User Profile'
      }}
    </v-card-title>
    <v-card-text>
      <div class="d-flex justify-center py-3 py-lg-5 flex-column align-center">
        <v-skeleton-loader type="avatar" v-if="!user.uid"></v-skeleton-loader>
        <v-avatar :size="$vuetify.breakpoint.mdAndDown ? 120 : 150" v-else>
          <v-img
            :src="user.avatar || require('@/assets/anon.png')"
            lazy-src="@/assets/anon_thumbnail.png"
            :alt="user.username"
          ></v-img>
        </v-avatar>
        <div class="mt-3 text--primary body-1 font-weight-medium"
             v-if="user.email && user.showEmail">{{ user.email }}
        </div>
        <div class="mt-2">Member since {{ formatDate(user.createdAt) }}</div>
      </div>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Me } from '@/services/model';
import { formatDate } from '@/composable/date';

export default defineComponent({
  name: 'UserSimpleCard',
  props: {
    user: Object as PropType<Me>,
  },
  setup() {
    return {
      formatDate,
    };
  },
});
</script>
