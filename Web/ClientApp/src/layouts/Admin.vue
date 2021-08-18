<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      app
    >
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="title">
            Dr.Forum
          </v-list-item-title>
          <v-list-item-subtitle>
            Doctors Forums
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>

      <v-list
        dense
        nav
      >
        <template v-for="item in links">

          <v-list-group
            v-if="item.links"
            :key="item.title"
            :prepend-icon="item.icon"
          >
            <template v-slot:activator>
              <v-list-item-title>Topic</v-list-item-title>
            </template>
            <v-list-item
              v-for="link in item.links"
              :key="link.title"
              :to="link.name ? { name: link.name } : null"
              exact-path
              link
            >
              <v-list-item-icon>
                <v-icon>{{ link.icon || 'dashboard' }}</v-icon>
              </v-list-item-icon>

              <v-list-item-content>
                <v-list-item-title>{{ link.title }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-group>

          <v-list-item
            v-else
            :key="item.title"
            :to="item.name ? { name: item.name } : null"
            exact-path
            link
          >
            <v-list-item-icon>
              <v-icon>{{ item.icon || 'dashboard' }}</v-icon>
            </v-list-item-icon>

            <v-list-item-content>
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app color="indigo" dark>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>{{ title }}</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <v-container class="mt-3 mt-md-4 mt-lg-5 mt-xl-6">
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';

export default defineComponent({
  name: 'Admin',
  setup() {
    const title = useTitle('Admin', { observe: true });
    const drawer = ref(true);

    const links = [
      { title: 'Dashboard', icon: 'bar_chart', name: 'Dashboard' },
      {
        title: 'Topics',
        icon: 'style',
        links: [
          { title: 'Category', icon: 'label_important', name: 'Dashboard' },
          { title: 'Tag', icon: 'local_offer', name: 'Dashboard' },
        ],
      },
      { title: 'Users', icon: 'person', name: 'Dashboard' },
    ];

    return {
      title,
      drawer,
      links,
    };
  },
});
</script>
